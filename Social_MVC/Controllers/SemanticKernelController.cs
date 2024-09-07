using Microsoft.AspNetCore.Mvc;
using Social_Models.Dto;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Social_MVC.Controllers;

public class SemanticKernelController : Controller
{
    private readonly Kernel _kernel;

    public SemanticKernelController(Kernel kernel)
    {
        _kernel = kernel;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(OpenAiChatRequestDto requestDto)
    {
        if (requestDto == null || string.IsNullOrEmpty(requestDto?.GenerateActivity))
        {
            throw new ArgumentException(nameof(requestDto.City));
        }

        var input = ConvertToKernelArguments(requestDto);

        var executionSettings = new OpenAIPromptExecutionSettings { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };
        var promptTemplate = $"Aktivitet {requestDto.GenerateActivity} i staden {requestDto.City}";
        var chatSemanticFunction = _kernel.CreateFunctionFromPrompt(promptTemplate, executionSettings);
        var chatPromptResult = await _kernel.InvokeAsync(chatSemanticFunction, input);

        ViewBag.Result = chatPromptResult.ToString();
        return View();
    }

    private KernelArguments ConvertToKernelArguments(OpenAiChatRequestDto requestDto)
    {
        var kernelArguments = new KernelArguments();
        kernelArguments.Add("GenerateActivity", requestDto.GenerateActivity);
        kernelArguments.Add("City", requestDto.City);
        return kernelArguments;
    }
}