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

    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', getLikes);
    xhr.open('POST', '/Like/GetLikes');
    xhr.send();
}

function getLikes() {
    likeList = JSON.parse(this.responseText);
    const item = document.getElementById("commentLikeBar");

    commentList.forEach((comment) => {
        if (likeList.length >= 1) {
            likeList.forEach((like) => {
                if (like.comment_ID == comment.id && like.owner_ID == item.dataset.ownerid) {
                    document.getElementById("dislike_" + comment.id).style.display = "block";
                    document.getElementById("like_" + comment.id).style.display = "none";
                }
                else {
                    document.getElementById("dislike_" + comment.id).style.display = "none";
                    document.getElementById("like_" + comment.id).style.display = "block";
                }
            });
        }
        else {
            document.getElementById("dislike_" + comment.id).style.display = "none";
            document.getElementById("like_" + comment.id).style.display = "block";
        }
    });
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
    const likes = JSON.parse(this.responseText);

    if (likes.length >= 1) {
        likes.forEach((like) => {
            if (like.owner_ID == item.dataset.ownerid && item.dataset.modelid == like.id) {
                if (item.dataset.table == "user" || item.dataset.table == "shop" || item.dataset.table == "vehicle") {
                    siteLikeSwitch(true);
                }
            }
            else {
                siteLikeSwitch(false);
            }
        });
    }
    else
        siteLikeSwitch(false);
}

function UserLikedThis() {
    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', checkLikes);
    xhr.open('POST', '/Like/GetLikes');
    xhr.send();
}

UserLikedThis();
var commentList;
var likeList;
let actualCommentID;
//
const xhr = new XMLHttpRequest();
xhr.addEventListener('load', getComments);
xhr.open('POST', '/Like/GetComments');
xhr.send();