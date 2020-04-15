using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GS.Chloe.Extension;
using GS.Common.Utils;
using GS.Entity.Customer;
using GS.Entity.Customer.DBContext;
using GS.Service.Customer.Bo;
using GS.Service.Customer.Enums;
using GS.Tookits.Base;
using GS.Tookits.Extension;
using GS.Tookits.Helper;
using Microsoft.Extensions.Configuration;

namespace GS.Service.Customer
{
    public class UserService : BaseService
    {
        private readonly string _url;
        public UserService(IConfiguration iConfiguration, PersonPlatformContext db) : base(db)
        {
            var iConfiguration1 = iConfiguration;
            _url = iConfiguration1["UserLogo"];
        }
        /// <summary>
        /// 获取根据条件用户列表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<User> GetUserList(Expression<Func<User, bool>> expression)
        {
            return Db.Query<User>().Where(expression).ToList();
        }
        /// <summary>
        /// 根据条件获取当前用户
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public User Get(Expression<Func<User, bool>> expression)
        {
            return Db.Query<User>().FirstOrDefault(expression);
        }

        /// <summary>
        /// 修改用户余额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public ServiceResult UpdateBalance(string userId, decimal money)
        {
            var user = Get(a => a.UserId == userId);
            var userMoney = user.Balance + money;
            Db.Update<User>(a => a.UserId == userId,
                a => new User { Balance = userMoney });

            return ServiceResult.IsSuccess("成功");
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ServiceResult<AdministratorData> RegisterUser(User user)
        {
            var userItem = Db.Query<User>().FirstOrDefault(a => a.UserNo == user.UserNo);
            if (userItem != null)
                return ServiceResult<AdministratorData>.IsFailed("会员ID重复,请重新注册！");

            var isAny = Db.Query<User>().Any(a => (a.UserName == user.UserName || a.Phone == user.Phone) && a.CompanyId == user.CompanyId);
            if (isAny)
                return ServiceResult<AdministratorData>.IsFailed("该用户名已被注册");

            if (string.IsNullOrEmpty(user.ImgUrl))
                user.ImgUrl = _url;

            user.UserId = GuidHelper.GenerateComb().ToString("N");
            user.Status = (int)PersonEnum.Status.Open;
            user.CreateDatetime = DateTime.Now;
            user.Password = EncodePassword(user.UserId, user.Password);
            if(string.IsNullOrEmpty(user.WxName))
                user.NickName = user.UserName;
            else
                user.NickName = user.WxName;
            Db.Insert(user);

            var retdData = new AdministratorData
            {
                UserId = user.UserId,
                CompanyId = user.CompanyId,
                UserNo = user.UserNo
            };
            return ServiceResult<AdministratorData>.IsSuccess("注册成功", retdData);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");

            var oldEncodePassword = EncodePassword(userId, oldPassword);
            var passwordForOldMd5 = EncodeOldPassword(oldPassword);

            if (user.Password != oldEncodePassword && user.Password != passwordForOldMd5)
                return ServiceResult.IsFailed("原密码错误");

            var newEncodePassword = EncodePassword(userId, newPassword);
            Db.Update<User>(a => a.UserId == userId, a => new User { Password = newEncodePassword });

            return ServiceResult.IsSuccess("修改密码完成");
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="phone"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult SetPassword(string companyId, string phone, string newPassword)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.Phone == phone && a.CompanyId == companyId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");

            var newEncodePassword = EncodePassword(user.UserId, newPassword);
            Db.Update<User>(a => a.UserId == user.UserId, a => new User { Password = newEncodePassword });

            return ServiceResult.IsSuccess("修改密码完成");
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <param name="password">密码</param>
        /// <param name="companyId">企业Id</param>
        /// <returns></returns>
        public ServiceResult<AdministratorData> LogonCheck(string input, string password, string companyId)
        {
            var user = Db.Query<User>().FirstOrDefault(a => (a.Phone == input || a.UserName == input || a.UserNo == input) && a.CompanyId == companyId);

            if (user == null)
                return ServiceResult<AdministratorData>.IsFailed("账户不存在");
            if (user.Status == (int)PersonEnum.Status.Stop)
                return ServiceResult<AdministratorData>.IsFailed("该账户已被停用");

            var passwordForMd5 = EncodePassword(user.UserId, password);
            var passwordForOldMd5 = EncodeOldPassword(password);
            if (user.Password != passwordForMd5 && user.Password != passwordForOldMd5)
                return ServiceResult<AdministratorData>.IsFailed("账户或密码错误！");

            return ServiceResult<AdministratorData>.IsSuccess("登录成功", new AdministratorData
            {
                UserId = user.UserId,
                CompanyId = user.CompanyId,
                UserNo = user.UserNo,
                UserName = user.UserName
            });
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <param name="countryCode"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ServiceResult UpdatePhone(string userId, string phone, string countryCode, string companyId)
        {
            var isCanUpdate = Db.Query<User>().Any(a => a.CompanyId == companyId && a.UserId != userId && a.Phone == phone);
            if (isCanUpdate)
                return ServiceResult.IsFailed("该手机号已绑定其他用户,无法在进行绑定.");

            var updateResult = Db.Update<User>(a => a.UserId == userId, a => new User { Phone = phone, CountryCode = countryCode }) > 0;
            if (updateResult)
                return ServiceResult.IsSuccess("修改绑定手机成功");

            return ServiceResult.IsFailed("账户不存在");
        }
        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="payPassword"></param>
        /// <returns></returns>
        public ServiceResult SetPayPassword(string userId, string payPassword)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");
            var newEncodePassword = EncodePassword(user.UserId, payPassword);
            Db.Update<User>(a => a.UserId == userId, a => new User { PayPassword = newEncodePassword });

            return ServiceResult.IsSuccess("设置支付密码成功");
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public ServiceResult ChangePayPassword(string userId, string oldPassword, string newPassword)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");

            var oldEncodePassword = EncodePassword(userId, oldPassword);
            if (user.PayPassword != oldEncodePassword)
                return ServiceResult.IsFailed("原密码错误");

            var newEncodePassword = EncodePassword(userId, newPassword);
            Db.Update<User>(a => a.UserId == userId, a => new User { PayPassword = newEncodePassword });

            return ServiceResult.IsSuccess("修改支付密码完成");
        }
        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="payPassword"></param>
        /// <returns></returns>
        public ServiceResult CheckingPayPassword(string userId, string payPassword)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");

            var oldEncodePassword = EncodePassword(userId, payPassword);
            if (user.PayPassword != oldEncodePassword)
                return ServiceResult.IsFailed("支付密码错误");
            return ServiceResult.IsSuccess("验证成功");
        }

        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ServiceResult<AdministratorData> WxLogonCheck(string openId, string companyId)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.OpenId == openId && a.CompanyId == companyId);
            if (user == null)
                return ServiceResult<AdministratorData>.IsFailed("账户不存在");
            if (user.Status == (int)PersonEnum.Status.Stop)
                return ServiceResult<AdministratorData>.IsFailed("该账户已被停用");
            return ServiceResult<AdministratorData>.IsSuccess("登录成功", new AdministratorData
            {
                UserId = user.UserId,
                CompanyId = user.CompanyId,
                UserNo = user.UserNo
            });
        }

        /// <summary>
        /// 更换或绑定微信
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="wxname"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ServiceResult BindingWx(string userId, string openId, string wxname, string companyId)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("账户不存在");
            var phoneUser = Db.Query<User>().FirstOrDefault(a => a.OpenId == openId && a.CompanyId == companyId);
            if (phoneUser == null || phoneUser.UserId == user.UserId)
            {
                Db.Update<User>(a => a.UserId == userId,
                    a => new User { OpenId = openId, WxName = wxname });
                return ServiceResult.IsSuccess("绑定微信成功");
            }
            return ServiceResult.IsFailed("该微信号已绑定其他用户,无法在进行绑定.");
        }

        #region 辅助方法

        public string EncodePassword(string userId, string password)
        {
            return (password.EncodeMd5String() + userId).EncodeMd5String();
        }

        public string EncodeOldPassword(string password)
        {
            return password.EncodeMd5String().ToLower().EncodeMd5String().ToLower();
        }



        #endregion

        #region 个人信息维护

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLogo"></param>
        /// <returns></returns>
        public ServiceResult UpdateUserLogo(string userId, string userLogo)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                return ServiceResult.IsFailed("用户不存在");
            }
            Db.Update<User>(u => u.UserId == userId, u => new User { ImgUrl = userLogo });
            return ServiceResult.IsSuccess("修改成功");
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public ServiceResult UpdateNickName(string userId, string nickName)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                return ServiceResult.IsFailed("用户不存在");
            }
            Db.Update<User>(u => u.UserId == userId, u => new User { NickName = nickName });
            return ServiceResult.IsSuccess("修改成功");
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ServiceResult UpdateUserName(string userId, string userName, string companyId)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
                return ServiceResult.IsFailed("无该账户");

            if (user.IsChangedUsername)
                return ServiceResult.IsFailed("只允许修改一次用户名");

            var isAny = Db.Query<User>().Any(a => a.CompanyId == companyId && a.UserId != userId && a.UserName == userName);
            if (isAny)
                return ServiceResult.IsFailed("该用户名已绑定其他用户,无法在进行绑定.");

            var updateResult = Db.Update<User>(a => a.UserId == userId, a => new User { UserName = userName, IsChangedUsername = true }) > 0;
            if (updateResult)
                return ServiceResult.IsSuccess("修改绑定用户名成功");

            return ServiceResult.IsFailed("账户不存在");
        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="realName"></param>
        /// <returns></returns>
        public ServiceResult UpdateRealName(string userId, string realName)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                return ServiceResult.IsFailed("用户不存在");
            }
            Db.Update<User>(u => u.UserId == userId, u => new User { RealName = realName });
            return ServiceResult.IsSuccess("修改成功");
        }

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ServiceResult UpdateEmail(string userId, string email)
        {
            var user = Db.Query<User>().FirstOrDefault(a => a.UserId == userId);
            if (user == null)
            {
                return ServiceResult.IsFailed("用户不存在");
            }
            Db.Update<User>(u => u.UserId == userId, u => new User { Email = email });
            return ServiceResult.IsSuccess("修改成功");
        }

        #endregion
    }
}
