layui.use(['form','layer','table','laytpl'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    //单位列表
    var tableIns = table.render({
        elem: '#unitList',
        url: '/Unit/LoadData',
        cellMinWidth : 95,
        page : true,
        height : "full-125",
        limits : [10,15,20,25],
        limit : 20,
        id : "unitListTable",
        cols : [[
            {type: "checkbox", fixed:"left", width:50},
            {field: 'Name', title: '名称', minWidth:100, align:"center"},
            {field: 'Address', title: '地址', minWidth:200, align:'center'},
            {field: 'Tel', title: '电话', align:'center'},
            {title: '操作', minWidth:50, templet:'#unitListBar',fixed:"right",align:"center"}
        ]]
    });

    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click",function(){
        if($(".searchVal").val() != '') {
            table.reload("unitListTable",
                {
                    page: {
                        curr: 1 //重新从第 1 页开始
                    },
                    where: {
                        key: $(".searchVal").val() //搜索的关键字
                    }
                });
        }else{
            layer.msg("请输入搜索的内容");
        }
    });
    //单位设备
    function deviceList(unitId) {
        var index = layui.layer.open({
            title: "设备清单",
            type: 2,
            content: "/Unit/DeviceList?unitId="+unitId,
            success: function (index) {
                var body = layui.layer.getChildFrame('body', index);
                body.find("#UnitId").val(unitId);
                setTimeout(function () {
                        layui.layer.tips('点击此处返回单位列表',
                            '.layui-layer-setwin .layui-layer-close',
                            {
                                tips: 3
                            });
                    },
                    500);
            }
        });
        layui.layer.full(index);
        window.sessionStorage.setItem("index", index);
        //改变窗口大小时，重置弹窗的宽高，防止超出可视区域（如F12调出debug的操作）
        $(window).on("resize",
            function () {
                layui.layer.full(window.sessionStorage.getItem("index"));
            });
    }

    //添加用户
    function addUnit(edit) {
        var id = 0;
        var tit="添加单位";
        if (edit) {
            id = edit.Id;
            tit = "编辑单位";
        }
        var index = layui.layer.open({
            title: tit,
            type: 2,
            content: "/Unit/Index",
            success: function(layero, index) {
                var body = layui.layer.getChildFrame('body', index);
                body.find("#Id").val(id);
                if (edit) {
                    body.find("#Name").val(edit.Name);
                    body.find("#Address").val(edit.Address);
                    body.find("#Tel").val(edit.Tel);
                    body.find("#LinkMan").val(edit.LinkMan);
                    body.find("#PhoneNum").val(edit.PhoneNum);
                    form.render();
                }
                setTimeout(function() {
                        layui.layer.tips('点击此处返回单位列表',
                            '.layui-layer-setwin .layui-layer-close',
                            {
                                tips: 3
                            });
                    },
                    500);
            }
        });
        layui.layer.full(index);
        window.sessionStorage.setItem("index",index);
        //改变窗口大小时，重置弹窗的宽高，防止超出可视区域（如F12调出debug的操作）
        $(window).on("resize",
            function() {
                layui.layer.full(window.sessionStorage.getItem("index"));
            });
    }

    $(".addUnit_btn").click(function() {
        addUnit();
    });

    //批量删除
    $(".delAll_btn").click(function() {
        var checkStatus = table.checkStatus('unitListTable'),
            data = checkStatus.data,
            unitIds = [];
        if (data.length > 0) {
            for (var i in data) {
                unitIds.push(data[i].newsId);
            }
            layer.confirm('确定删除选中的单位？',
                { icon: 3, title: '提示信息' },
                function(index) {
                    del(unitIds);
                });
        } else {
            layer.msg("请选择需要删除的单位");
        }
    });


    function del(unitId) {
        $.ajax({
            type: 'POST',
            url: '/Unit/Delete/',
            data: { ids: unitId },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (data) {//data为相应体,function为回调函数
                if (data.status === "ok") {
                    layer.msg("操作成功", {
                        time: 2000 //2s后自动关闭
                    }, function () {
                        tableIns.reload();
                        layer.close();
                    });
                } else {
                    layer.msg(data.errorMsg, {
                        time: 2000 //2s后自动关闭
                    }, function () {
                        tableIns.reload();
                        layer.close();
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert('操作失败！！！' + XMLHttpRequest.status + "|" + XMLHttpRequest.readyState + "|" + textStatus, { icon: 5 });
            }
        });
    }



    //列表操作
    table.on('tool(unitList)', function(obj){
        var layEvent = obj.event,
            data = obj.data;

        if (layEvent === 'edit') { //编辑
            addUnit(data);
        } else if (layEvent === 'del') { //删除
            layer.confirm('确定删除此单位？', { icon: 3, title: '提示信息' }, function (index) {
                del(data.Id);
            });
        } else if (layEvent === "deviceList") {//查看设备
            deviceList(data.Id);
        }
    });

})
