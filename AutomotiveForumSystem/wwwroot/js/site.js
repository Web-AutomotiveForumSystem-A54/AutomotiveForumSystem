function updateClock() {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');

    const timeString = `${hours}:${minutes}`;
    document.getElementById('clock').textContent = timeString;
}

setInterval(updateClock, 1000);

updateClock();

function setEditComment(commentId, commentContent) {
        $('#editCommentContent').val(commentContent);
        $('#editCommentForm').attr('action', '/Posts/UpdateComment?commentId=' + commentId);
        $('#editComment').modal('show');
    }