using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Survey.Areas.Admin.ViewModels;
using Survey.Core.Entities;
using Survey.Core.Interfaces;
using Survey.Infrastructure.Repository;

namespace Survey.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("Survey")]
	[Authorize]
	public class SurveyController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly ISurveyRepository _surveyRepository;
		private readonly ISectionRepository _sectionRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;

		public SurveyController()
			: this(new UserRepository(), new SurveyRepository(), new SectionRepository(), new QuestionRepository(), new AnswerRepository())
		{

		}

		public SurveyController(IUserRepository userRepository, ISurveyRepository surveyRepository,
								ISectionRepository sectionRepository, IQuestionRepository questionRepository,
								IAnswerRepository answerRepository)
		{
			_userRepository = userRepository;
			_surveyRepository = surveyRepository;
			_sectionRepository = sectionRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
		}

		// GET: Admin/Survey
		[Route("")]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Index()
		{
			var model = await _surveyRepository.GetAll();
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}

		// GET: Admin/Survey/Create
		[Route("Survey")]
		[Authorize(Roles = "admin,user")]
		public async Task<ActionResult> Survey(string surveyName)
		{
			var survey = await _surveyRepository.Get(surveyName);
			var model = new SurveyViewModel { Surveyses = await _surveyRepository.GetAll(), SurveyTitle = survey.Name,SurveyDescription = survey.Description};
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}
		// Post: Admin/Survey/Create
		[Route("Survey/{surveyName}")]
		[Authorize(Roles = "admin,user")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Survey(SurveyViewModel surveys, string surveyName)
		{
			var model = new SurveyViewModel { Surveyses = await _surveyRepository.GetAll() };
			var user = await GetloggedInUser();
			var survey = new TBL_Surveys();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			try
			{
				if (!ModelState.IsValid)
				{
					ModelState.AddModelError(string.Empty, "لطفا مقدار های مناسب پر کنید");
				}
				if (string.IsNullOrEmpty(surveyName))
				{
					survey = await _surveyRepository.Get(surveys.SurveyTitle);
				}
				else
				{
					survey = await _surveyRepository.Get(surveyName);
				}

				if (survey == null)
				{
					await _surveyRepository.Create(new TBL_Surveys
					{
						Name = surveys.SurveyTitle,
						Description = surveys.SurveyDescription,
						User_Id = user.Id
					});
					return RedirectToAction("Section", new { surveyName = surveys.SurveyTitle });
				}
				model.SurveyTitle = survey.Name;

				return RedirectToAction("Section", new { surveyName = surveys.SurveyTitle });
				//return View(model: model);
			}
			catch (Exception e)
			{

				ModelState.AddModelError(string.Empty, e.Message);

				return View(model: model);
			}

		}

		// GET: Admin/Section/Create
		[Route("Section")]
		[Authorize(Roles = "admin,user")]
		public async Task<ActionResult> Section(string surveyName)
		{
			var model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAll(), SurveyTitle = surveyName };
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}
		// Post: Admin/Section/Create
		[Route("Section/{sectionName}")]
		[Authorize(Roles = "admin,user")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Section(SurveyViewModel sections, string sectionName)
		{
			var model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAll() };
			var user = await GetloggedInUser();
			var survey = await _surveyRepository.Get(sections.SurveyTitle);
			var section = new TBL_Sections();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			try
			{
				if (!ModelState.IsValid)
				{
					ModelState.AddModelError(string.Empty, "لطفا مقدار های مناسب پر کنید");
				}
				if (string.IsNullOrEmpty(sectionName))
				{
					section = await _sectionRepository.Get(sections.SectionTitle, survey.Id);
				}
				else
				{
					section = await _sectionRepository.Get(sectionName, survey.Id);
				}

				if (section == null)
				{
					await _sectionRepository.Create(new TBL_Sections
					{
						Name = sections.SectionTitle,
						Survey_Id = survey.Id
					});
					return RedirectToAction("QuestionAnswer", new { surveyName = survey.Name, sectionName = sections.SectionTitle });
				}
				model.SurveyTitle = survey.Name;

				return View(model: model);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);

				return View(model: model);
			}

		}

		// GET: Admin/Section/Create
		[Route("QuestionAnswer")]
		[Authorize(Roles = "admin,user")]
		public async Task<ActionResult> QuestionAnswer(string surveyName, string sectionName)
		{
			var model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAll(), SurveyTitle = surveyName, SectionTitle = sectionName };
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}
		// Post: Admin/Section/Create
		[Route("QuestionAnswer/{sectionName}")]
		[Authorize(Roles = "admin,user")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> QuestionAnswer(SurveyViewModel questions, string questionName, HttpPostedFileBase fileUpload)
		{
			var model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAll() };
			var user = await GetloggedInUser();
			var survey = await _surveyRepository.Get(questions.SurveyTitle);
			var section = await _sectionRepository.Get(questions.SectionTitle, survey.Id);
			var question = new TBL_Questions();
			var answer = new TBL_Answers();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			try
			{
				if (!ModelState.IsValid)
				{
					ModelState.AddModelError(string.Empty, "لطفا مقدار های مناسب پر کنید");
				}
				if (string.IsNullOrEmpty(questionName))
				{
					question = await _questionRepository.Get(questions.QuestionTitle, section.Id);
				}
				else
				{
					question = await _questionRepository.Get(questionName, section.Id);
				}

				if (question == null)
				{
					var modelQuestion = new TBL_Questions
					{
						Title = questions.QuestionTitle,
						Section_Id = section.Id,
						Description = questions.QuestionDescription
					};
					var allowedExtensions = new[] {
						".Jpg", ".png", ".jpg", "jpeg"
					};

					var fileName = Path.GetFileName(fileUpload.FileName);
					var ext = Path.GetExtension(fileUpload.FileName); //getting the extension(ex-.jpg)
					if (allowedExtensions.Contains(ext)) //check what type of extension
					{
						string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
						string myfile = name + "_" + modelQuestion.Title + ext; //appending the name with id
						// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
						var path = Path.Combine(Server.MapPath("~/App_Data/images"), myfile);
						modelQuestion.ImageUrl = "~/App_Data/images" + myfile;
						fileUpload.SaveAs(path);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Please choose only Image file");
					}

					await _questionRepository.Create(modelQuestion);
					//return RedirectToAction();
				}
				model.SurveyTitle = survey.Name;

				return View(model: model);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);

				return View(model: model);
			}

		}

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{

			if (!_isDisposed)
			{
				_userRepository.Dispose();
			}
			_isDisposed = true;

			base.Dispose(disposing);
		}
		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _userRepository.GetUserByNameAsync(User.Identity.Name);
		}

		#endregion
	}
}