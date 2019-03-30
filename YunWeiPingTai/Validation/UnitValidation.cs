using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using YunWeiPingTai.Models;

namespace YunWeiPingTai.Validation
{
    public class UnitValidation:AbstractValidator<UnitAddEditPostModel>
    {
        public UnitValidation()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotEmpty().Length(2, 50).WithMessage("名称不能为空");
            RuleFor(x => x.Address).NotEmpty().Length(2, 200).WithMessage("地址不能为空");
            RuleFor(x => x.Tel).NotEmpty().Length(7, 13).WithMessage("联系电话不能为空");
            RuleFor(x => x.LinkMan).NotEmpty().Length(2, 10).WithMessage("联系人不能为空");
            RuleFor(x => x.PhoneNum).NotEmpty().Length(11).WithMessage("手机号不能为空");
        }
    }
}
