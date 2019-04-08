layui.use(['form','layer','table','laytpl'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    //单位列表
    var tableIns = table.render({
        elem: '#deviceList',
        url: '/Device/LoadData',
        cellMinWidth : 95,
        page : true,
        height : "full-125",
        limits : [10,15,20,25],
        limit : 20,
        id : "deviceListTable",
        cols : [[
            {type: "checkbox", fixed:"left", width:50},
            {field: 'Name', title: '名称', minWidth:100, align:"center"},
            {field: 'Version', title: '型号', minWidth:100, align:'center'},
            {field: 'Maker', title: '制造商', align:'center'},
            {title: '操作', minWidth:50, templet:'#deviceListBar',fixed:"right",align:"center"}
        ]]
    });

    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click",function(){
        if($(".searchVal").val() != '') {
            table.reload("deviceListTable",
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

    //添加用户
    function addDevice(edit) {
        var id = 0;
        var tit="添加设备";
        if (edit) {
            id = edit.Id;
            tit = "编辑设备";
        }
        var index = layui.layer.open({
            title: tit,
            type: 2,
            content: "/Device/Index",
            success: function(layero, index) {
                var body = layui.layer.getChildFrame('body', index);
                body.find("#Id").val(id);
                if (edit) {
                    body.find("#Name").val(edit.Name);
                    body.find("#Version").val(edit.Version);
                    body.find("#Maker").val(edit.Maker);
                    form.render();
                }
                setTimeout(function() {
                        layui.layer.tips('点击此处返回设备列表',
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

    $(".addDevice_btn").click(function() {
        addDevice();
    });

    //批量删除
    $(".delAll_btn").click(function() {
        var checkStatus = table.checkStatus('deviceListTable'),
            data = checkStatus.data,
            newsId = [];
        if (data.length > 0) {
            for (var i in data) {
                newsId.push(data[i].newsId);
            }
            layer.confirm('确定删除选中的设备？',
                { icon: 3, title: '提示信息' },
                function(index) {
                    // $.get("删除文章接口",{
                    //     newsId : newsId  //将需要删除的newsId作为参数传入
                    // },function(data){
                    tableIns.reload();
                    layer.close(index);
                    // })
                });
        } else {
            layer.msg("请选择需要删除的设备");
        }
    });


    function del(deviceIds) {
        $.ajax({
            type: 'POST',
            url: '/Device/Delete/',
            data: { ids: deviceIds },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (data) {//data为相应体,function为回调函数
                if (data.status === "ok") {

                } else {
                    layer.msg(data.errorMsg, {
                        time: 2000 //2s后自动关闭
                    }, function () {
                        tableIns.reload();
                        layer.close(index);
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.alert('操作失败！！！' + XMLHttpRequest.status + "|" + XMLHttpRequest.readyState + "|" + textStatus, { icon: 5 });
            }
        });
    }



    //列表操作
    table.on('tool(deviceList)', function(obj){
        var layEvent = obj.event,
            data = obj.data;

        if(layEvent === 'edit'){ //编辑
            addDevice(data);
        }else if(layEvent === 'del'){ //删除
            layer.confirm('确定删除此设备？',{icon:3, title:'提示信息'},function(index) {
                del(data.Id);
            });
        }
    });

})
