using System.ComponentModel.DataAnnotations;
using FytSoa.Application.Sys;
using FytSoa.Common.Enum;
using FytSoa.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FytSoa.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly SysMessageService _messageService;
    public IndexModel(ILogger<IndexModel> logger
    ,SysMessageService messageService)
    {
        _logger = logger;
        _messageService = messageService;
    }

    public void OnGet()
    {
    }
    
    [BindProperty]
    public ContactForm contactForm { get; set; } = new ContactForm();
    
    /// <summary>
    /// 接收联系我们信息的AJAX POST接口
    /// </summary>
    /// <returns>JSON响应</returns>
    public async Task<IActionResult> OnPostSubmitContact()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return new JsonResult(new
                {
                    success = false,
                    message = "提交失败，参数验证错误",
                    errors = errors
                });
            }

            await _messageService.AddAsync(new SysMessageDto()
            {
                Types = MessageEnum.System,
                UserName = contactForm.Name,
                Title=contactForm.Service,
                Email=contactForm.Email,
                Mobile = contactForm.Phone,
                Summary = contactForm.Message,
            });
            return new JsonResult(new
            {
                success = true,
                ticket=StringUtils.RandomString(),
                message = "感谢您的留言，我们会尽快联系您！"
            });
        }
        catch (Exception ex)
        {
            return new JsonResult(new
            {
                success = false,
                message = "服务器处理失败，请稍后重试",
                errorDetail = ex.Message 
            });
        }
    }
    
    // 联系表单参数模型（带数据验证）
    public class ContactForm
    {
        [Required(ErrorMessage = "姓名不能为空")]
        [MaxLength(50, ErrorMessage = "姓名长度不能超过50个字符")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "手机号不能为空")]
        [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "服务类型不能为空")]
        public string Service { get; set; } = string.Empty;

        [Required(ErrorMessage = "留言内容不能为空")]
        [MinLength(10, ErrorMessage = "留言内容不能少于10个字符")]
        public string Message { get; set; } = string.Empty;
    }
}