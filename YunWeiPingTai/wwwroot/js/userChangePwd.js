layui.use(['form','layer','laydate','table','laytpl'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laydate = layui.laydate,
        laytpl = layui.laytpl,
        table = layui.table;

    //添加验证规则
    form.verify({
        oldPwd: function(value, item) {

        },
        newPwd: function(value, item) {
            if (value.length < 6) {
                return "密码长度不能小于6位";
            }
        },
        confirmPwd: function(value, item) {
            if (!new RegExp($("#newPwd").val()).test(value)) {
                return "两次输入密码不一致，请重新输入！";
            }
        }
    });
    form.on("submit(changePwd)", function (data) {
        var obj = $(this);
        obj.text("提交中...").attr("disabled", "disabled").addClass("layui-disabled");
        //获取防伪标记
        $.ajax({
            type: 'POST',
            url: '/Home/ChangePwd/',
            data: data.field,
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (res) {//res为相应体,function为回调函数
                if (res.status ==="ok") {
                    layer.alert("修改成功，请重新登录", { icon: 1 }, function (index) {
                        layer.close(index);
                        parent.location.href = "/Account/Logout";

                    });

                } else {
                    layer.alert(res.ErrorMsg, { icon: 5 }, function (index) {
                        layer.close(index);
                        location.reload();
                    });
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert('操作失败！！！' + XMLHttpRequest.status + "|" + XMLHttpRequest.readyState + "|" + textStatus, { icon: 5 });
            },
            complete: function () {
                obj.text("登录").removeAttr("disabled").removeClass("layui-disabled");

            }
        });
        return false;
    });

})