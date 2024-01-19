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

function setEditPost(postId, postTitle, postContent) {
    $('#editPostTitle').val(postTitle);
    $('#editPostContent').val(postContent);
    $('#editPostForm').attr('action', '/Posts/UpdatePost?postId=' + postId);
    $('#editPost').modal('show');
}

function displayTags(tagsJsonString, containerId) {
    // Parse JSON string into JavaScript array
    var tagsArray = JSON.parse(tagsJsonString);

    // Display tags using JavaScript
    var tagsContainer = $('#' + containerId);
    for (var i = 0; i < tagsArray.length; i++) {
        tagsContainer.append($('<div class="badge bg-danger m-1">').text(tagsArray[i]));
    }
}

function updateDisplayedTags() {
    var inputText = $('#tagsInput').val();

    // Get the tags container
    var tagsContainer = $('#displayedTagsContainer');

    // Clear previous tags
    tagsContainer.empty();

    // Parse JSON string into JavaScript array (assuming you have the JSON string stored in a hidden field)
    var tagsJsonString = $('#tagsJson').val();
    var tagsArray = JSON.parse(tagsJsonString);

    // Display matching tags
    for (var i = 0; i < tagsArray.length; i++) {
        if (tagsArray[i].toLowerCase().includes(inputText.toLowerCase())) {
            tagsContainer.append($('<div class="badge bg-danger m-1">').text(tagsArray[i]));
        }
    }
}