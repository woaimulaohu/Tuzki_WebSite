//--------------留言板相关--start----------------------
//更新留言板
function getBoard(pageStartNum, pageSize, postId, isUpdataPage, action) {
    if (isNullOrEmpty(pageStartNum) || pageStartNum < 1) {
        pageStartNum = 1;
    }
    if (isNullOrEmpty(pageSize)) {
        pageSize = 5;
    }
    $.post(action, {
        'pageStartNum': pageStartNum,
        'pageSize': pageSize,
        'postId': postId
    }, function (data) {
        $("#MsgBoard").html(data);
        if (isUpdataPage == 1) {
            getBoardPaging(pageStartNum, pageSize, postId, action);
        }
        //设置当前选中页蓝色背景
        $("#boardPage li").each(function () {
            $(this).attr("class", "");
            if ($(this).text() == pageStartNum) {
                $(this).attr("class", "active");
            }
        });
    });
}
//获取页码
function getBoardPaging(pageStartNum, pageSize, postId, action) {
    if (isNullOrEmpty(pageStartNum) || pageStartNum < 1) {
        pageStartNum = 1;
    }
    if (isNullOrEmpty(pageSize)) {
        pageSize = 5;
    }
    var pageEndNum = pageSize + pageStartNum;
    //生成当前页码
    var startBack = parseInt(pageStartNum) - parseInt(pageSize) < 0 ? 1 : parseInt(pageStartNum) - parseInt(pageSize);
    var html = '<li><a href="javascript:void(0)" onclick="getBoard(' + parseInt(startBack) + ',' + parseInt(pageSize) + ',' + postId + ',' + 1 + ",'" + action + "'" + ')"><<</a></li>';
    for (var i = pageStartNum; i < pageEndNum; i++) {
        if (i == pageStartNum) {
            html += '<li class="active"><a href="javascript:void(0)" onclick="getBoard(' + i + ',' + parseInt(pageSize) + ',' + postId + ',' + 0 + ",'" + action + "'" + ')">' + i + '</a></li>';
        } else {
            html += '<li><a href="javascript:void(0)" onclick="getBoard(' + i + ',' + parseInt(pageSize) + ',' + postId + ',' + 0 + ",'" + action + "'" + ')">' + i + '</a></li>';
        }
    }
    var startForward = parseInt(pageStartNum) + parseInt(pageSize);
    html += '<li><a href="javascript:void(0)" onclick="getBoard(' + parseInt(startForward) + ',' + parseInt(pageSize) + ',' + postId + ',' + 1 + ",'" + action + "'" + ')">>></a></li>';
    $('#boardPage').html(html);
}
//留言
function leaveComment(postId, beforCommentsId, action) {
    if ($('#nickName').val().length > 30) {
        tips('boardForm', "昵称过长", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    if ($('#nickName').val().length == 0) {
        tips('boardForm', "不想给自己来个炫酷的昵称么", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    if ($('#comment').val().length == 0) {
        tips('boardForm', "不想说点什么么", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    $('#tips').remove();
    $('.emojioneemoji').css("width", "22px");
    $('.emojioneemoji').css("height", "22px");
    $.post('Blog/leaveComment', {
        'nickName': $('#nickName').val(),
        'comment': encodeURI($('#comment').next().children(".emojionearea-editor").html()),
        'beforCommentsId': beforCommentsId,
        "avatarUrl": $('#avatarUrl').val(),
        'postId': postId,
    }, function (data) {
        getBoard('', '', postId, 0, action);
        tips('boardForm', "感谢你的留言", 'success');
        setTimeout("$('#tips').remove()", 5000);
        $('#comment').val('')
    });
}
//--------------留言板相关--end----------------------

//-------------指定留言回复--start--------------------
//回复某一条留言弹出留言框
function reply(postId, beforCommentsId, action) {
    var id = postId + '' + beforCommentsId;
    var lastId = $("#lastReplyId").val();
    $('#' + lastId + '_r').remove();
    $('#' + id).append('<div class="row"  id=' + id + '_r>' +
'<form role="form" id="boardForm_r" style="margin-bottom:20px; margin-top: 20px;margin-left: 100px;width:350px">' +
'<input id="avatarUrl_r" style="display:none" />' +
    '<div class="form-group">' +
    '<input type="text" class="form-control" id="nickName_r" placeholder="你的昵称">' +
   '</div>' +
   '<div class="form-group">' +
    '<textarea class="form-control" id="comment_r" placeholder="我有话说"></textarea>' +
   '</div>' +
 '<button type="button" class="btn btn-info" onclick="leaveReply(' + postId + ',' + beforCommentsId + ',\'' + action + '\')">说完了</button>' +
'</form></div>');
    $("#avatarUrl_r").val(getAvatar());
    $("#nickName_r").val(getUserName());
    $("#nickName_r").attr("placeholder", getUserName());
    $("#lastReplyId").val(id);
    $("#boardForm_r").blur(function () {
        $('#' + id + '_r').remove();
    });
    $("#email_r").focus(function () {
        tips('boardForm_r', "支持gravatar头像功能,通用的email与头像关联方案,可以访问<a>http://cn.gravatar.com</a> 设置头像", 'info');
    });
    $("#comment_r").emojioneArea({
        autoHideFilters: true
    });
}
//回复指定留言确定按钮触发
function leaveReply(postId, beforCommentsId, action) {
    if ($('#nickName_r').val().length > 30) {
        tips('boardForm_r', "昵称过长", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    if ($('#nickName_r').val().length == 0) {
        tips('boardForm_r', "不想给自己来个炫酷的昵称么", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    if ($('#comment_r').val().length == 0) {
        tips('boardForm_r', "不想说点什么么", 'info');
        setTimeout("$('#tips').remove()", 5000);
        return;
    }
    $('#tips').remove();
    $('.emojioneemoji').css("width", "22px");
    $('.emojioneemoji').css("height", "22px");
    $.post('Blog/leaveComment', {
        "avatarUrl": $('#avatarUrl_r').val(),
        'nickName': $('#nickName_r').val(),
        'comment': encodeURI($('#comment_r').next().children(".emojionearea-editor").html()),
        'beforCommentsId': beforCommentsId,
        'postId': postId,
    }, function (data) {
        getBoard('', '', postId, 0, action);
        tips('boardForm_r', "感谢你的留言", 'success');
        setTimeout("$('#tips').remove()", 5000);
    });
}
//-------------指定留言回复--end--------------------
//-------------登录相关----start--------------------
//退出登录
function quitLogin() {
    $.cookie('userInfo', null);
    $.cookie('token', null);
    getHome();
}
//获取cookie用户名
function getUserName() {
    return $.cookie('userInfo').split('&')[0].split('=')[1];
}
//获取cookie用户头像
function getAvatar() {
    var avatar = $.cookie('userInfo').substr($.cookie('userInfo').indexOf('&') + 1);
    return avatar.substr(avatar.indexOf('=') + 1, avatar.length - avatar.indexOf('='));
}
//检查是否登录(登录成功会在cookie中保存token)
function checkLogin() {
    $.post('Home/CheckLogin', { 'token': $.cookie('token') }, function (data) {
        if (data == 'fail') {
            jumpClick('../Other/Index');
        } else {
            $("#login").hide();
            $("#login_Info").show();
            $("#login_avatar").attr("src", getAvatar());
            $("#login_nickName").text(getUserName());
        }
    });
}
//播放下一首歌曲
function audioNext() {
    $.post('Home/AudioNext?index=' + $("#audioPlayer").attr("name"), function (data) {
        var arr = new Array();
        arr = data.split(',');
        $("#audioPlayer").attr("src", arr[1]);
        $("#audioPlayer").attr("name", arr[0]);
    });
}
//浮动提示框
function tips(elementId, msg, alertLevel) {
    var html;
    elementId = '#' + elementId;
    if ("danger" == alertLevel) {
        html = ' <div class="alert alert-danger" id="tips" role="alert"><strong>错误!</strong>' + msg + '</div>';
    }
    if ("warning" == alertLevel) {
        html = ' <div class="alert alert-warning" id="tips" role="alert"><strong>警告!</strong>' + msg + '</div>';
    }
    if ("success" == alertLevel) {
        html = ' <div class="alert alert-success" id="tips" role="alert"><strong>成功!</strong>' + msg + '</div>';
    }
    if ("info" == alertLevel) {
        html = ' <div class="alert alert-info" id="tips" role="alert"><strong>提示!</strong>' + msg + '</div>';
    }
    $('#tips').remove();
    $(elementId).append(html);
}
//-------------登录相关----end--------------------