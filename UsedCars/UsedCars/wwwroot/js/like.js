function dislikeSite() {
    document.getElementById("dislike").style.display = "none";
    document.getElementById("like").style.display = "block";
    $.post("/Like/Delete", { 'likedata': [@user.ID.ToString(), @Model.ID.ToString(), '@(currentTable)']
});
document.getElementById("siteLikeCount").innerHTML = @(Model.Likes.Count) - 1;
    }

function likeSite() {
    document.getElementById("like").style.display = "none";
    document.getElementById("dislike").style.display = "block";
    $.post("/Like/Create", { 'likedata': [@user.ID.ToString(), @Model.ID.ToString(), '@(currentTable)']
});
document.getElementById("siteLikeCount").innerHTML = @(Model.Likes.Count) + 1;
    }

function dislikeComment(id) {
    document.getElementById("dislike_" + id).style.display = "none";
    document.getElementById("like_" + id).style.display = "block";
    $.post("/Like/Delete", { 'likedata': [@user.ID.ToString(), id, 'comment']
});
document.getElementById("likecount_" + id).innerHTML = CountCommentLikes(id);
    }

function likeComment(id) {
    document.getElementById("dislike_" + id).style.display = "block";
    document.getElementById("like_" + id).style.display = "none";
    $.post("/Like/Create", { 'likedata': [@user.ID.ToString(), id, 'comment']
});
document.getElementById("likecount_" + id).innerHTML = CountCommentLikes(id) + 1;
    }

function CountCommentLikes(id) {
    let count = 0;
    likeList.forEach((like) => {
        if (like.comment_ID == id) {
            count++;
        }
    });
    return count;
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
    commentList.forEach((comment) => {
        if (likeList.length >= 1) {
            likeList.forEach((like) => {
                if (like.comment_ID == comment.id && like.owner_ID == @user.ID) {
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

let check = '@(Singleton.UserIsLikedThis(user.ID, Model.ID, currentTable))'

if (check === 'False') {
    document.getElementById("dislike").style.display = "none";
    document.getElementById("like").style.display = "block";
}
else {
    document.getElementById("dislike").style.display = "block";
    document.getElementById("like").style.display = "none";
}

var commentList;
var likeList;
const xhr = new XMLHttpRequest();
xhr.addEventListener('load', getComments);
xhr.open('POST', '/Like/GetComments');
xhr.send();