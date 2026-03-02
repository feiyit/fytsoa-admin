#nullable enable
using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace FytSoa.Domain.Sys;

/// <summary>
/// 密码重置令牌表。
/// </summary>
[SugarTable("sys_password_reset_token")]
public class SysPasswordResetToken
{
    [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    [Required]
    [SugarColumn(ColumnName = "tenant_id")]
    public long TenantId { get; set; }

    [Required]
    [SugarColumn(ColumnName = "user_id")]
    public long UserId { get; set; }

    [Required]
    [StringLength(255)]
    [SugarColumn(ColumnName = "token_hash")]
    public string TokenHash { get; set; } = string.Empty;

    [SugarColumn(ColumnName = "expires_at")]
    public DateTime ExpiresAt { get; set; }

    [SugarColumn(ColumnName = "used_at")]
    public DateTime? UsedAt { get; set; }

    [SugarColumn(ColumnName = "created_at")]
    public DateTime CreatedAt { get; set; }
}
