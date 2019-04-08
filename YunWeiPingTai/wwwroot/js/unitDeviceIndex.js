
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
                url: '/Unit/DeviceIndex/',
                data: {
                    Id: $("#Id").val(),
                    UnitId: $("#UnitId").val(),
                    DeviceId: $("#DeviceId").val(),
                    SNCode: $("#SNCode").val()
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
});