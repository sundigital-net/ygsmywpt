
layui.config({
    base: "/js/"
}).extend({
    "authtree": "authtree"
});
layui.use(['form', 'layer', 'authtree'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery, authtree = layui.authtree;
    form.on("submit(addUnit)", function (data) {
        //获取防伪标记
            $.ajax({
                type: 'POST',
                url: '/Unit/Index/',
                data: {
                    Id: $("#Id").val(),
                    Name: $("#Name").val(),
                    Address: $("#Address").val(),
                    Tel: $("#Tel").val(),
                    LinkMan: $("#LinkMan").val(),
                    PhoneNum: $("#PhoneNum").val()
            },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (res) {//res为响应体,function为回调函数
                if (res.status ==="ok") {
                    var alertIndex01 = layer.alert("操作成功", { icon: 1 }, function () {
                        layer.closeAll("iframe");
                        //刷新父页面
                        parent.location.reload();
                        top.layer.close(alertIndex01);
                    });
                    //$("#res").click();//调用重置按钮将表单数据清空
                } else {
                    var alertIndex02 =  layer.alert(res.errorMsg, { icon: 5 }, function () {
                        layer.closeAll("iframe");
                        //刷新父页面
                        parent.location.reload();
                        top.layer.close(alertIndex02);
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert('操作失败！！！' + XMLHttpRequest.status + "|" + XMLHttpRequest.readyState + "|" + textStatus, { icon: 5 });
            }
        });
        return false;
    });
    form.verify({
        Name: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '单位名称不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '单位名称首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '单位名称不能全为数字';
            }
            var msg;
            $.ajax({
                url: "/Unit/IsExistsName/",
                async: false,
                data: {
                    Name: value,
                    Id: $("#Id").val()
                },
                dataType: 'json',
                success: function (res) {
                    if (res.Data === true) {
                        msg = "系统已存在相同的单位名称，请修改后再进行操作";
                    }
                },
                error: function (xml, errstr, err) {
                    msg = "系统异常，请稍候再试";
                }
            });
            if (msg) {
                return msg;
            }
        }
    });
});