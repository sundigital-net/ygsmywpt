layui.use(['form','layer','jquery'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

   

    //登录按钮
    form.on("submit(login)",
        function(data) {
            var obj = $(this);
            obj.text("登录中...").attr("disabled", "disabled").addClass("layui-disabled");
            //登录过程

            var formData = $("#login-form").serializeArray();
            $.ajax({
                url: "/Account/Login",
                type: "POST",
                dataType: "json",
                //data: formData,
                data: data.field,
                headers: {
                    "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
                },
                success: function(res) {
                    if (res.status == "ok") {
                        layer.msg("登录成功");
                        setTimeout(function() {
                                window.location.href = "/Home/Index";
                            },
                            1000);
                        //location.href = "/Home/Index";
                    } else {
                        layer.msg(res.errorMsg);
                        //location.reload();
                        $("#loginCaptcha").click();

                    }
                },
                error: function(res) {
                    layer.msg("网络连接异常");
                    //location.reload();
                    $("#loginCaptcha").click();

                },
                complete: function() {
                    obj.text("登录").removeAttr("disabled").removeClass("layui-disabled");

                }
            });


            /*setTimeout(function(){
                window.location.href = "/Home/Index";
            },1000);*/
            return false;
        });
    form.verify({
        userName: function (value, item) {//value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '用户名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '用户名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '用户名不能全为数字';
            }
            if (value.length > 32 || value.length < 2) {
                return '用户名长度必须符合规则';
            }
        },
        captcha: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '验证码不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '验证码首尾不能出现下划线\'_\'';
            }
            if (value.length !== 4) {
                return '验证码长度必须符合规则';
            }
        },
        password: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '密码首尾不能出现下划线\'_\'';
            }
            if (value.length > 32 || value.length < 6) {
                return '验证码长度必须符合规则';
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
