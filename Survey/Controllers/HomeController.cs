using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Survey.Core.Entities;
using Survey.Core.Interfaces;
using Survey.Infrastructure.Repository;
using Survey.ViewModels;

namespace Survey.Controllers
{
	[RoutePrefix("")]
	public class HomeController : Controller
	{
		private readonly IRespondentRepository _respondentRepository;
		private readonly ISurveyRepository _surveyRepository;
		private readonly ISectionRepository _sectionRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IRSQARepository _rsqaRepository;

		public HomeController()
			: this(new RespondentRepository(), new SurveyRepository(), new SectionRepository(),
				   new QuestionRepository(), new AnswerRepository(), new RSQARepository())
		{

		}

		public HomeController(IRespondentRepository respondentRepository, ISurveyRepository surveyRepository, ISectionRepository sectionRepository,
			IQuestionRepository questionRepository, IAnswerRepository answerRepository, IRSQARepository rsqaRepository)
		{
			_respondentRepository = respondentRepository;
			_surveyRepository = surveyRepository;
			_sectionRepository = sectionRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_rsqaRepository = rsqaRepository;
		}

		// GET: Home
		[AllowAnonymous]
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var survey = await _surveyRepository.Get("صمیمت بین زوجین");

			ViewBag.PTitle = survey.Name;
			ViewBag.PDescription = survey.Description;

			return View();
		}

		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<ActionResult> RegisterRespondent(TBL_Respondents model, string survey)
		{
			try
			{
				await _respondentRepository.Create(model);

				return RedirectToAction("Exam", new { survey = survey, index = 0, email = model.Email });
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(viewName: "Index");
			}
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<ActionResult> Exam(string survey, int index, string email)
		{
			var model = new List<ExamViewModel>();

			var sections = await _sectionRepository.GetAllBySurveyName(survey);
			var sectionArray = await GetSection(survey, index);
			var surveyModel = await _surveyRepository.Get(survey);
			var section = await _sectionRepository.Get(sectionArray, surveyModel.Id);

			if (sections.Count == index)
			{
				return RedirectToAction("Complete");
			}

			var questions = await _questionRepository.GetAllBySectionName(await GetSection(survey, index));

			foreach (var item in questions)
			{
				var answers = await _answerRepository.GetAllByQuestionName(item.Title);
				var answerArray = new string[answers.Count];

				var i = 0;
				foreach (var ans in answers)
				{
					answerArray[i] = ans.Text;
					i++;
				}

				var answer = string.Join("-", answerArray);

				model.Add(new ExamViewModel
				{
					QuestionTitle = item.Title,
					QuestionDescription = item.Description,
					QuestionImgeUrl = item.ImageUrl,
					Answer = answer,
				});
			}

			ViewBag.ListQA = model;
			ViewBag.SurveyTitle = survey;
			ViewBag.PTitle = section.Name;
			ViewBag.PDescription = section.Description;
			ViewBag.Index = index;
			ViewBag.Email = email;
			ViewBag.Total = sections.Count;
			ViewBag.Current = index + 1;
			return View();
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> Exam(IList<ExamViewModel> model)
		{
			try
			{
				string survey = String.Empty;
				string email = string.Empty;
				int index = 0;

				foreach (var item in model)
				{
					survey = item.Survey;
					email = item.Email;
					index = item.Index;
					var indexTemp = index - 1;

					var sectionArrayIndex = await GetSection(item.Survey, indexTemp);


					var surveyModel = await _surveyRepository.Get(item.Survey);
					var sectionModel = await _sectionRepository.Get(sectionArrayIndex, surveyModel.Id);
					var questionModel = await _questionRepository.Get(item.QuestionTitle, sectionModel.Id);
					var answerModel = await _answerRepository.Get(item.Answer, questionModel.Id);
					var respondent = await _respondentRepository.Get(item.Email);

					await _rsqaRepository.Create(new TBL_RSQA
					{
						Survey_Id = surveyModel.Id,
						Section_Id = sectionModel.Id,
						Question_Id = questionModel.Id,
						Answer_Id = answerModel.Id,
						Respondent_Id = respondent.Id
					});
				}

				return RedirectToAction("Exam", new { survey = survey, index = index, email = email });
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return View();
			}
		}


		public async Task<ActionResult> Complete()
		{
			return View();
		}


		#region Method

		private async Task<string> GetSection(string survey, int index)
		{
			var sections = await _sectionRepository.GetAllBySurveyName(survey);
			var sectionArray = new string[sections.Count];

			var i = 0;
			foreach (var item in sections)
			{
				sectionArray[i] = item.Name;
				i++;
			}

			return sectionArray[index];
		}

		#endregion
	}
}