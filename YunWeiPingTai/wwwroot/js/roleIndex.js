
layui.config({
    base: "/js/"
}).extend({
    "authtree": "authtree"
});
layui.use(['form', 'layer', 'authtree'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery, authtree = layui.authtree;

    form.on("submit(addRole)", function (data) {
        //获取防伪标记
        $.ajax({
            type: 'POST',
            url: '/Role/Index/',
            data: {
                Id: $("#Id").val(),  //主键
                Name: $("#Name").val(),  //角色名称
                MenuIds: authtree.getChecked('#sundigital-auth-tree'),
                Remark: $("#Remark").val()  //用户简介
            },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (res) {//res为相应体,function为回调函数
                if (res.status==="ok") {
                    var alertIndex = layer.alert("操作成功", { icon: 1 }, function () {
                        layer.closeAll("iframe");
                        //刷新父页面
                        parent.location.reload();
                        top.layer.close(alertIndex);
                    });
                    //$("#res").click();//调用重置按钮将表单数据清空
                } else {
                    var alertIndex1=layer.alert(res.errorMsg, { icon: 5 }, function () {
                        layer.closeAll("iframe");
                        //刷新父页面
                        parent.location.reload();
                        top.layer.close(alertIndex1);
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
        roleName: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '角色名称不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '角色名称首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '角色名称不能全为数字';
            }
            var msg;
            $.ajax({
                url: "/Role/IsExistsName/",
                async: false,
                data: {
                    Name: value,
                    Id: $("#Id").val()
                },
                dataType: 'json',
                success: function (res) {
                    if (res === true) {
                        msg = "系统已存在相同的角色名称，请修改后再进行操作";
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

    // 初始化
    $.ajax({
        url: '/Menu/LoadDataWithParentId/',
        dataType: 'json',
        success: function (data) {
            // 渲染时传入渲染目标ID，树形结构数据（具体结构看样例，checked表示默认选中），以及input表单的名字
            var trees = authtree.listConvert(data, {
                primaryKey: 'Id'
                , startPid: 0
                , parentKey: 'ParentId'
                , nameKey: 'DisplayName'
                , valueKey: 'Id'
                , checkedKey: strToIntArr($('#MenuIdsInit').val())
            });
            authtree.render('#sundigital-auth-tree', trees, {
                inputname: 'ids[]'
                , layfilter: 'sundigital-check-auth'
                , autowidth: true
            });

            authtree.on('change(sundigital-check-auth)', function (data) {
                console.log('监听 authtree 触发事件数据', data);
            });
            authtree.on('dblclick(sundigital-check-auth)', function (data) {
                console.log('监听到双击事件', data);
            });
        },
        error: function (xml, errstr, err) {
            layer.alert(errstr + '，系统异常！');
        }
    });

    function strToIntArr(str) {
        if (str) {
            var strArr = str.split(',');
            var dataIntArr = [];//保存转换后的整型字符串
            //方法一
            strArr.forEach(function (data, index, arr) {
                dataIntArr.push(+data);
            });
            return dataIntArr;
        }
    }
});