layui.use(['form', 'layer'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;

    form.on("submit(addManager)", function (data) {
        //获取防伪标记
        $.ajax({
            type: 'POST',
            url: '/User/Index/',
            data: {
                Id: $("#Id").val(),  //主键
                UserName: $("#UserName").val(), 
                RoleId: $("#RoleId").val(),  
               
                PhoneNum: $("#PhoneNum").val(), 
                Email: $("#Email").val(), 
                IsLock: $("#IsLock").get(0).checked
            },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (res) {//res为相应体,function为回调函数
                if (res.status === "ok") {
                    var alertIndex = layer.alert('操作成功,新增用户默认密码为手机号后6位', { icon: 1 }, function () {
                        layer.closeAll("iframe");
                        //刷新父页面
                        parent.location.reload();
                        top.layer.close(alertIndex);
                    });
                    //$("#res").click();//调用重置按钮将表单数据清空
                }
                else {
                    layer.alert(res.errorMsg, { icon: 5 });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert('操作失败！！！' + XMLHttpRequest.status + "|" + XMLHttpRequest.readyState + "|" + textStatus, { icon: 5 });
            }
        });
        return false;
    });
    form.verify({
        username: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '用户名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '用户名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '用户名不能全为数字';
            }
        },
        userphone: function(value,item) {
            var msg;
            $.ajax({
                url: "/User/IsExistsAccount/",
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
        useremail: function (value, item) {
            var msg;
            $.ajax({
                url: "/User/IsExistsAccount/",
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
        //我们既支持上述函数式的方式，也支持下述数组的形式
        //数组的两个值分别代表：[正则匹配、匹配不符时的提示文字]
        pass: [
            /^[\S]{6,12}$/
            , '密码必须6到12位，且不能出现空格'
        ]
    });      
});