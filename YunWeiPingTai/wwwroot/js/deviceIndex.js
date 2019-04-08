
layui.config({
    base: "/js/"
}).extend({
    "authtree": "authtree"
});
layui.use(['form', 'layer', 'authtree'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery, authtree = layui.authtree;
    form.on("submit(addDevice)", function (data) {
        //获取防伪标记
        $.ajax({
            type: 'POST',
            url: '/Device/Index/',
            data: {
                Id: $("#Id").val(),  //主键
                Name: $("#Name").val(),
                Version: $("#Version").val(),
                Maker: $("#Maker").val()
            },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (res) {//res为相应体,function为回调函数
                if (res.status === "ok") {
                    var alertIndex = layer.alert("操作成功", { icon: 1 }, function () {
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
        deviceName: function (value, item) { //value：表单的值、item：表单的DOM对象
            
            var msg;
            $.ajax({
                url: "/Device/IsExistsName/",
                async: false,
                data: {
                    Name: value,
                    Id: $("#Id").val()
                },
                dataType: 'json',
                success: function (res) {
                    if (res === true) {
                        msg= "系统已存在相同的设备名称，请修改后再进行操作";
                    }
                },
                error: function (xml, errstr, err) {
                    msg= "系统异常，请稍候再试";
                }
            });
            if (msg) {
                return msg;
            }
        }
    });      
});