layui.use(['form','layer','jquery'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

   

    //注册按钮
    form.on("submit(register)",
        function(data) {
            var btn = $(this);
            btn.text("注册中...").attr("disabled", "disabled").addClass("layui-disabled");
            //注册过程

            //var formData = $("#reg-form").serializeArray();
            $.ajax({
                url: "/Account/Register",
                type: "POST",
                dataType: "json",
                //data: formData,
                data: data.field,
                success: function(res) {
                    if (res.status === "ok") {
                        layer.msg("注册成功，正在登录...");
                        setTimeout(function() {
                                window.location.href = "/Home/Index";
                            },
                            1000);
                    } else {
                        layer.msg(res.errorMsg);
                        $("#loginCaptcha").click();

                    }
                },
                complete: function() {
                    btn.text("注册").removeClass("layui-disabled").removeAttr("disabled");
                },
                error: function(res) {
                    layer.msg("网络连接异常");
                    $("#loginCaptcha").click();
                }
            });


            /*setTimeout(function(){
                window.location.href = "/Home/Index";
            },1000);*/
            return false;
        });
    form.verify({
        userPhone: function(value, item) {
            var msg;
            $.ajax({
                url: "/Account/IsExistsAccount/",
                async: false,
                data: {
                    Account: value,
                    Id: $("#Id").val()
                },
                dataType: 'json',
                success: function (res) {
                    if (res === true) {
                        msg = "系统已存在相同的账号，请修改后再进行操作";
                    }
                },
                error: function (xml, errstr, err) {
                    msg = "系统异常，请稍候再试";
                }
            });
            if (msg) {
                return msg;
            }
        },
        userEmail: function (value, item) {
            var msg;
            $.ajax({
                url: "/Account/IsExistsAccount/",
                async: false,
                data: {
                    Account: value,
                    Id: $("#Id").val()
                },
                dataType: 'json',
                success: function (res) {
                    if (res === true) {
                        msg = "系统已存在相同的账号，请修改后再进行操作";
                    }
                },
                error: function (xml, errstr, err) {
                    msg = "系统异常，请稍候再试";
                }
            });
            if (msg) {
                return msg;
            }
        },
        pwd: function(value, item) {
            if (!new RegExp("(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9]).{6,30}").test(value)) {
                return '密码必须包含大、小写字母以及数字，且长度大于6并小于30';
            }
        }
    });
    //表单输入效果
    $(".loginBody .input-item").click(function(e) {
        e.stopPropagation();
        $(this).addClass("layui-input-focus").find(".layui-input").focus();
    });
    $(".loginBody .layui-form-item .layui-input").focus(function() {
        $(this).parent().addClass("layui-input-focus");
    });
    $(".loginBody .layui-form-item .layui-input").blur(function() {
        $(this).parent().removeClass("layui-input-focus");
        if ($(this).val() != '') {
            $(this).parent().addClass("layui-input-active");
        } else {
            $(this).parent().removeClass("layui-input-active");
        }
    });

    //验证码
    $("#loginCaptcha").click(function () {
        $("#loginCaptcha").attr("src", "/Account/GetCaptcha?t=" + Math.random());
    });
    $("#loginCaptcha").click();//避免部分浏览器刚打开时加载上次的缓存验证码
})
