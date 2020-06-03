function fakeButton() {
    document.getElementById("fake").style.display = "inherit";
}

function dislikeSite() {
    siteLikeSwitch(false);
    const item = document.getElementById("siteLikeBar");

    $.post("/Like/Delete", {
        'likedata': [item.dataset.ownerid, item.dataset.modelid, item.dataset.table]
    })
        .done(function () {
            getLikesXHR(true);
        });
}

function likeSite() {
    siteLikeSwitch(true);
    const item = document.getElementById("siteLikeBar");

    $.post("/Like/Create", {
        'likedata': [item.dataset.ownerid, item.dataset.modelid, item.dataset.table]
    })
        .done(function () {
            getLikesXHR(true);
        });
}

function getLikesXHR(mode) {
    const xhr = new XMLHttpRequest();

    if (mode)
        xhr.addEventListener('load', onSiteLikeChange);
    else
        xhr.addEventListener('load', onCommentLikeChange);

    xhr.open('POST', '/Like/GetLikes');
    xhr.send();
}

function onSiteLikeChange() {
    const likes = JSON.parse(this.responseText);
    const item = document.getElementById("siteLikeBar");
    let count = 0;

    likes.forEach((like) => {
        if (item.dataset.table == "user" && like.user_ID >= 1 ||
            item.dataset.table == "shop" && like.shop_ID >= 1 ||
            item.dataset.table == "vehicle" && like.vehicle_ID >= 1) {
            count++;
        }
    });
    document.getElementById("siteLikeCount").innerHTML = count;
}

function dislikeComment(id) {
    document.getElementById("dislike_" + id).style.display = "none";
    document.getElementById("like_" + id).style.display = "block";
    const item = document.getElementById("commentLikeBar");
    actualCommentID = id;

    $.post("/Like/Delete", {
        'likedata': [item.dataset.ownerid, id, 'comment']
    })
        .done(function () {
            //EZT MEGKÉRDEZNI!!!
            getLikesXHR(false);
        });
}

function likeComment(id) {
    document.getElementById("dislike_" + id).style.display = "block";
    document.getElementById("like_" + id).style.display = "none";
    const item = document.getElementById("commentLikeBar");
    actualCommentID = id;

    $.post("/Like/Create", {
        'likedata': [item.dataset.ownerid, id, 'comment']
    })
        .done(function () {
            getLikesXHR(false);
        });
}

function onCommentLikeChange() {
    const likes = JSON.parse(this.responseText);
    const item = document.getElementById("commentLikeBar");
    let count = 0;

    likes.forEach((like) => {
        if (like.comment_ID == actualCommentID) {
            count++;
        }
    });
    document.getElementById("likecount_" + actualCommentID).innerHTML = count;
}

function getComments() {
    commentList = JSON.parse(this.responseText);
    UserLikedThis();
}

function checkCommentLikes() {
    const item = document.getElementById("commentLikeBar");

    //console.log("comments length = " + commentList.length);
    commentList.forEach(comment => {
        console.log("commentid = " + comment.id);
        if (likeList.length >= 1) {
            likeList.forEach(like => {
                if (like.comment_ID == comment.id && like.owner_ID == item.dataset.ownerid) {
                    console.log("bekapcs " + like.id);
                    commentLikeSwitch(true, comment.id);
                }
                else {
                    console.log("kikapcs " + like.id);
                    commentLikeSwitch(false, comment.id);
                }
            });
        }
        else {
            console.log("nem lép be");
            commentLikeSwitch(false, comment.id);
        }
    });
}

function commentLikeSwitch(mode, id) {
    if (mode) {
        document.getElementById("dislike_" + id).style.display = "block";
        document.getElementById("like_" + id).style.display = "none";
    }
    else {
        document.getElementById("dislike_" + id).style.display = "none";
        document.getElementById("like_" + id).style.display = "block";
    }
}

function siteLikeSwitch(mode) {
    if (mode) {
        document.getElementById("dislike").style.display = "block";
        document.getElementById("like").style.display = "none";
    }
    else {
        document.getElementById("dislike").style.display = "none";
        document.getElementById("like").style.display = "block";
    }
}

function checkLikes() {
    const item = document.getElementById("siteLikeBar");
    likeList = JSON.parse(this.responseText);
    var table = [];

    if (likeList.length >= 1) {
        likeList.forEach((like) => {
            if (like.owner_ID == item.dataset.ownerid && item.dataset.table == "user" && item.dataset.modelid == like.user_ID ||
                like.owner_ID == item.dataset.ownerid && item.dataset.table == "shop" && item.dataset.modelid == like.shop_ID ||
                like.owner_ID == item.dataset.ownerid && item.dataset.table == "vehicle" && item.dataset.modelid == like.vehicle_ID) {
                table.push(true);
            }
            else {
                table.push(false);
            }
        });
    }
    else
        siteLikeSwitch(false);

    if (table.includes(true)) {
        siteLikeSwitch(true);
    }
    else {
        siteLikeSwitch(false);
    }

    checkCommentLikes();
}

function UserLikedThis() {
    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', checkLikes);
    xhr.open('POST', '/Like/GetLikes');
    xhr.send();
}

var commentList;
var likeList;
let actualCommentID;
//
const xhr = new XMLHttpRequest();
xhr.addEventListener('load', getComments);
xhr.open('POST', '/Like/GetComments');
xhr.send();