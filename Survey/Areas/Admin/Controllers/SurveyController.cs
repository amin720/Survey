using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Survey.Areas.Admin.Models;
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
		private readonly IRSQARepository _rsqaRepository;
		private readonly IRespondentRepository _respondentRepository;

		public SurveyController()
			: this(new UserRepository(), new SurveyRepository(), new SectionRepository(),
				  new QuestionRepository(), new AnswerRepository(), new RSQARepository(),
				  new RespondentRepository())
		{

		}

		public SurveyController(IUserRepository userRepository, ISurveyRepository surveyRepository,
								ISectionRepository sectionRepository, IQuestionRepository questionRepository,
								IAnswerRepository answerRepository, IRSQARepository rsqaRepository,
								IRespondentRepository respondentRepository)
		{
			_userRepository = userRepository;
			_surveyRepository = surveyRepository;
			_sectionRepository = sectionRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_rsqaRepository = rsqaRepository;
			_respondentRepository = respondentRepository;
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
			var model = new SurveyViewModel();
			if (survey != null)
			{
				model = new SurveyViewModel
				{
					Surveyses = await _surveyRepository.GetAll(),
					SurveyTitle = survey.Name,
					SurveyDescription = survey.Description,
					IsDisplay = survey.IsDisplay
				};
			}
			else
			{
				model.Surveyses = await _surveyRepository.GetAll();
			}

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
		public async Task<ActionResult> Survey(SurveyViewModel surveys)
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

				if (string.IsNullOrEmpty(surveys.SurveyTitle))
				{
					surveys.SurveyTitle = surveys.NewSurveyTitle;
				}

				survey = await _surveyRepository.Get(surveys.SurveyTitle);


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
				else
				{
					survey.Name = (surveys.SurveyTitle == surveys.NewSurveyTitle? surveys.SurveyTitle:surveys.NewSurveyTitle);
					survey.Description = surveys.SurveyDescription;
					survey.IsDisplay = surveys.IsDisplay;
					await _surveyRepository.Edit(survey, surveys.SurveyTitle);
				}
				model.SurveyTitle = survey.Name;

				return RedirectToAction("Section", new { surveyName = model.SurveyTitle });
				//return View(model: model);
			}
			catch (Exception e)
			{

				ModelState.AddModelError(string.Empty, e.Message);

				return View(model: model);
			}

		}

		// GET: Admin/Survey/Create
		[Route("SurveyDelete/{surveyName}")]
		[Authorize(Roles = "admin,user")]
		public async Task<ActionResult> SurveyDelete(string surveyName)
		{
			var survey = await _surveyRepository.Get(surveyName);
			var model = new TBL_Surveys
			{
				Name = survey.Name,
				Description = survey.Description,
				User_Id = survey.User_Id
			};

			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}
		// Post: Admin/Survey/Create
		[Route("SurveyDelete")]
		[Authorize(Roles = "admin,user")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SurveyDelete(TBL_Surveys survey)
		{
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;
			var model = survey;
			try
			{
				await _surveyRepository.Delete(survey.Name);

				return RedirectToAction("Survey", new { surveyName = survey.Name });
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
		public async Task<ActionResult> Section(string surveyName, string sectionName)
		{
			var model = new SurveyViewModel();
			if (!string.IsNullOrEmpty(surveyName) && string.IsNullOrEmpty(sectionName))
			{
				model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAllBySurveyName(surveyName), SurveyTitle = surveyName };
			}
			else if (!string.IsNullOrEmpty(sectionName))
			{
				var survey = await _surveyRepository.Get(surveyName);
				var section = await _sectionRepository.Get(sectionName, survey.Id);
				model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAllBySurveyName(surveyName), SurveyTitle = surveyName, SectionTitle = section.Name, SectionDescription = section.Description };
			}

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
		public async Task<ActionResult> Section(SurveyViewModel sections)
		{
			var model = new SurveyViewModel { Sectionses = await _sectionRepository.GetAllBySurveyName(sections.SurveyTitle) };
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

				section = await _sectionRepository.Get(sections.SectionTitle, survey.Id);

				if (section == null)
				{
					await _sectionRepository.Create(new TBL_Sections
					{
						Name = sections.SectionTitle,
						Description = sections.SectionDescription,
						Survey_Id = survey.Id
					});
					return RedirectToAction("QuestionAnswer", new { surveyName = survey.Name, sectionName = sections.SectionTitle });
				}
				else
				{
					section.Name = sections.NewSectionTitle;
					section.Description = sections.SectionDescription;

					await _sectionRepository.Edit(section,sections.SectionTitle);
				}
				model.SurveyTitle = survey.Name;

				return RedirectToAction("QuestionAnswer", new { surveyName = sections.SurveyTitle, sectionName = sections.SectionTitle });
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
		public async Task<ActionResult> QuestionAnswer(string surveyName, string sectionName, string questinoTitle)
		{
			var model = new SurveyViewModel();
			if (!string.IsNullOrEmpty(surveyName) && !string.IsNullOrEmpty(sectionName) && string.IsNullOrEmpty(questinoTitle))
			{
				model = new SurveyViewModel { Questions = await _questionRepository.GetAllBySectionName(sectionName), SurveyTitle = surveyName, SectionTitle = sectionName };
			}
			else if (!string.IsNullOrEmpty(questinoTitle))
			{
				var survey = new TBL_Surveys();
				if (!string.IsNullOrEmpty(surveyName))
				{
					survey = await _surveyRepository.Get(surveyName);
				}

				var section = await _sectionRepository.Get(Convert.ToInt32(sectionName), survey.Id);
				var question = await _questionRepository.Get(questinoTitle, section.Id);
				var answerses = await _answerRepository.GetAllByQuestionName(question.Title);
				var answers = new string[answerses.Count];

				int i = 0;
				foreach (var item in answerses)
				{
					answers[i] = item.Text;
					i++;
				}

				model = new SurveyViewModel
				{
					Questions = await _questionRepository.GetAllBySectionName(section.Name),
					Answerses = answerses,
					SurveyTitle = survey.Name,
					SectionTitle = section.Name,
					QuestionTitle = question.Title,
					QuestionDescription = question.Description,
					QuestionImageUrl = question.ImageUrl,
					StartLabel = question.StartLabel,
					EndLabel = question.EndLabel,
					Option = string.Join("-", answers)
				};
			}

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
		public async Task<ActionResult> QuestionAnswer(SurveyViewModel questions, HttpPostedFileBase filePicker)
		{
			var model = new SurveyViewModel { Questions = await _questionRepository.GetAllBySectionName(questions.SectionTitle) };
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

				question = await _questionRepository.Get(questions.QuestionTitle, section.Id);

				var answers = questions.Option.Split('-');

				var modelQuestion = new TBL_Questions
				{
					Title = questions.QuestionTitle,
					Section_Id = section.Id,
					Description = questions.QuestionDescription,
					StartLabel = questions.StartLabel,
					EndLabel = questions.EndLabel,
				};

				if (question == null)
				{
					if (filePicker != null)
					{
						var allowedExtensions = new[] {
							".Jpg", ".png", ".jpg", "jpeg"
						};

						var fileName = Path.GetFileName(filePicker.FileName);
						var ext = Path.GetExtension(filePicker.FileName); //getting the extension(ex-.jpg)
						if (allowedExtensions.Contains(ext)) //check what type of extension
						{
							string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
							string myfile = name + "_" + modelQuestion.Title + ext; //appending the name with id
																					// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
							var path = Path.Combine(Server.MapPath("~/images/"), myfile);
							modelQuestion.ImageUrl = "~/images/" + myfile;
							filePicker.SaveAs(path);
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Please choose only Image file");
						}
					}

					await _questionRepository.Create(modelQuestion);

					question = await _questionRepository.Get(questions.QuestionTitle, section.Id);

					foreach (var item in answers)
					{
						await _answerRepository.Create(new TBL_Answers
						{
							Text = item,
							Question_Id = question.Id
						});
					}
				}
				else
				{
					question.Title = questions.NewQuestionTitle;
					question.Description = questions.QuestionDescription;
					question.ImageUrl = questions.QuestionImageUrl;
					question.StartLabel = questions.StartLabel;
					question.EndLabel = questions.EndLabel;
					
					if (filePicker != null)
					{
						var allowedExtensions = new[] {
							".Jpg", ".png", ".jpg", "jpeg"
						};

						var fileName = Path.GetFileName(filePicker.FileName);
						var ext = Path.GetExtension(filePicker.FileName); //getting the extension(ex-.jpg)
						if (allowedExtensions.Contains(ext)) //check what type of extension
						{
							string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
							string myfile = name + "_" + question.Title + ext; //appending the name with id
																					// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
							var path = Path.Combine(Server.MapPath("~/images/"), myfile);
							question.ImageUrl = "~/images/" + myfile;
							filePicker.SaveAs(path);
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Please choose only Image file");
						}
					}

					await _questionRepository.Edit(question,questions.QuestionTitle);

					question = await _questionRepository.Get(questions.QuestionTitle, section.Id);

					foreach (var item in answers)
					{
						await _answerRepository.Edit(new TBL_Answers
						{
							Text = item,
							Question_Id = question.Id
						}, item);
					}
				}

				question = await _questionRepository.Get(questions.QuestionTitle, section.Id);

				model.SurveyTitle = survey.Name;
				model.SectionTitle = section.Name;
				model.QuestionTitle = question.Title;
				model.QuestionDescription = question.Description;
				model.QuestionImageUrl = question.ImageUrl;
				model.Option = string.Join("-", answers);
				model.Questions = await _questionRepository.GetAllBySectionName(section.Name);

				return View(model: model);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);

				return View(model: model);
			}

		}






		public async Task<ActionResult> ExportToExcel()
		{
			var export = new List<Export2Excel>();
			var rsqa = await _rsqaRepository.GetAll();

			foreach (var item in rsqa)
			{
				var survey = await _surveyRepository.Get(item.Survey_Id);
				var section = await _sectionRepository.Get(item.Section_Id, item.Survey_Id);
				var question = await _questionRepository.Get(item.Question_Id, item.Section_Id);
				var answer = await _answerRepository.Get(item.Answer_Id, item.Question_Id);
				var respondent = await _respondentRepository.Get(item.Respondent_Id);

				export.Add(new Export2Excel
				{
					Survey = survey.Name,
					Section = section.Name,
					Question = question.Title + question.Description,
					Answer = answer.Text,
					Degree = respondent.Degree,
					Gender = respondent.Gender == 0? "مرد":"زن",
					Age = respondent.Age
				});
			}

			var gv = new GridView();
			gv.DataSource = export;
			gv.DataBind();
			Response.ClearContent();
			Response.Buffer = true;
			Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
			Response.ContentType = "application/ms-excel";
			Response.Charset = "";
			StringWriter objStringWriter = new StringWriter();
			HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
			gv.RenderControl(objHtmlTextWriter);
			Response.Output.Write(objStringWriter.ToString());
			Response.Flush();
			Response.End();
			return View("Survey");
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
		private static string GetBetween(string strSource, string strStart, string strEnd)
		{
			int Start, End;
			if (strSource.Contains(strStart) && strSource.Contains(strEnd))
			{
				Start = strSource.IndexOf(strStart, 0) + strStart.Length;
				End = strSource.IndexOf(strEnd, Start);
				return strSource.Substring(Start, End - Start);
			}
			else
			{
				return "";
			}
		}

		#endregion
	}
}