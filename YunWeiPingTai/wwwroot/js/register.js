layui.use(['form','layer','jquery'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer
        $ = layui.jquery;

   

    //登录按钮
    form.on("submit(register)", function (data) {
        var btn = $(this);
        btn.text("注册中...").attr("disabled", "disabled").addClass("layui-disabled");
        //登录过程

        var formData = $("#reg-form").serializeArray();
        $.ajax({
            url: "/Account/Register",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (res) {
                if (res.status == "ok") {
                    layer.msg("注册成功，正在登录...");
                    setTimeout(function () {
                        window.location.href = "/Home/Index";
                    }, 1000);
                    //location.href = "/Home/Index";
                }
                else {
                    layer.msg(res.errorMsg);
                    //location.reload();
                    $("#loginCaptcha").click();
                    btn.text("注册").removeClass("layui-disabled").removeAttr("disabled");
                }
            },
            error: function (res) {
                layer.msg("网络连接异常");
                //location.reload();
                $("#loginCaptcha").click();
                btn.text("注册").removeClass("layui-disabled").removeAttr("disabled");
            }
        });


        /*setTimeout(function(){
            window.location.href = "/Home/Index";
        },1000);*/
        return false;
    })

    //表单输入效果
    $(".loginBody .input-item").click(function(e){
        e.stopPropagation();
        $(this).addClass("layui-input-focus").find(".layui-input").focus();
    })
    $(".loginBody .layui-form-item .layui-input").focus(function(){
        $(this).parent().addClass("layui-input-focus");
    })
    $(".loginBody .layui-form-item .layui-input").blur(function(){
        $(this).parent().removeClass("layui-input-focus");
        if($(this).val() != ''){
            $(this).parent().addClass("layui-input-active");
        }else{
            $(this).parent().removeClass("layui-input-active");
        }
    })

    //验证码
    $("#loginCaptcha").click(function () {
        $("#loginCaptcha").attr("src", "/Account/GetCaptcha?t=" + Math.random());
    });
    $("#loginCaptcha").click();//避免部分浏览器刚打开时加载上次的缓存验证码
})
