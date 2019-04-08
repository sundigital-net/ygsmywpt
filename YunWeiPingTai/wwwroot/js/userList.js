layui.use(['form','layer','table','laytpl'],function(){
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;

    //用户列表
    var tableIns = table.render({
        elem: '#userList',
        url: '../User/LoadData',
        cellMinWidth : 95,
        page : true,
        height : "full-125",
        limits : [10,15,20,25],
        limit : 20,
        id : "userListTable",
        cols : [[
            {type: "checkbox", fixed:"left", width:50},
            {field: 'Name', title: '姓名', minWidth:100, align:"center"},
            {field: 'Email', title: '用户邮箱', minWidth:200, align:'center'},
            {field: 'PhoneNum', title: '电话', align:'center'},
            { field: 'RoleName', title: '用户角色', align: 'center' },
            { field: 'IsLock', title: '是否禁用', minWidth: 100, align: "center", templet: '#IsLock' },
            { field: 'LastSigninTimeStr', title: '最后登录时间', align:'center',minWidth:150},
            {title: '操作', minWidth:75, templet:'#userListBar',fixed:"right",align:"center"}
        ]]
    });

    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click",function(){
        if($(".searchVal").val() != '') {
            table.reload("userListTable",
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
    function addUser(edit) {
        var id = 0;
        var tit = "添加用户";
        if (edit) {
            id = edit.Id;
            tit = "编辑用户";
        }
        var index = layui.layer.open({
            title: tit,
            type: 2,
            content: "/User/Index",
            success: function(layero, index) {
                var body = layui.layer.getChildFrame('body', index);
                body.find("#Id").val(id);
                if (edit) {
                    
                    body.find("#UserName").val(edit.Name);
                    body.find("#RoleId").val(edit.RoleId);
                    body.find("#PhoneNum").val(edit.PhoneNum);
                    body.find("#Email").val(edit.Email);
                    body.find("input:checkbox[name='IsLock']").prop("checked", edit.IsLock);
                    form.render();
                }
                setTimeout(function() {
                        layui.layer.tips('点击此处返回用户列表',
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

    $(".addUser_btn").click(function() {
        addUser();
    });

    //批量删除
    $(".delAll_btn").click(function() {
        var checkStatus = table.checkStatus('userListTable'),
            data = checkStatus.data,
            userId = [];
        if (data.length > 0) {
            for (var i in data) {
                userId.push(data[i].Id);
            }
            layer.confirm('确定删除选中的用户？',
                { icon: 3, title: '提示信息' },
                function(index) {
                    del(userId);
                });
        } else {
            layer.msg("请选择需要删除的用户");
        }
    });

    //列表操作
    table.on('tool(userList)', function(obj){
        var layEvent = obj.event,
            data = obj.data;

        if(layEvent === 'edit'){ //编辑
            addUser(data);
        }else if(layEvent === 'del'){ //删除
            layer.confirm('确定删除此用户？',{icon:3, title:'提示信息'},function(index) {
                del(data.Id);
            });
        }
    });
    //锁定
    form.on('switch(IsLock)', function(data) {
        var tipText = '确定锁定当前用户吗？';
        if (!data.elem.checked) {
            tipText = '确定启用当前用户吗？';
        }
        layer.confirm(tipText, {
            icon: 3,
            title: '系统提示',
            cancel: function (index) {
                data.elem.checked = !data.elem.checked;
                form.render();
                layer.close(index);
            }
        }, function (index) {
            changeLockStatus(data.value, data.elem.checked);
            layer.close(index);
        }, function (index) {
            data.elem.checked = !data.elem.checked;
            form.render();
            layer.close(index);
        });
    });
    //删除
    function del(userId) {
        $.ajax({
            type: 'POST',
            url: '/User/Delete/',
            data: { userId: userId },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (data) {//res为相应体,function为回调函数
                if (data.status === "ok") {
                    layer.msg("操作成功",
                        {
                            time: 2000 //20s后自动关闭
                        },
                        function() {
                            tableIns.reload();
                            layer.close();
                        });
                } else {
                    layer.msg(data.errorMsg, {
                        time: 2000 //20s后自动关闭
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
    function changeLockStatus(userId, status) {
        $.ajax({
            type: 'POST',
            url: '/User/ChangeLockStatus/',
            data: { id: userId, status: status },
            dataType: "json",
            headers: {
                "X-CSRF-TOKEN-sundigital": $("input[name='AntiforgeryKey_sundigital']").val()
            },
            success: function (data) {//res为相应体,function为回调函数
                if (data.status === "ok") {
                    layer.msg("操作成功",
                        {
                            time: 2000 //20s后自动关闭
                        },
                        function () {
                            tableIns.reload();
                            layer.close();
                        });
                } else {
                    layer.msg(data.errorMsg, {
                        time: 2000 //20s后自动关闭
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

})
