function updateClock() {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');

    const timeString = `${hours}:${minutes}`;
    document.getElementById('clock').textContent = timeString;
}

setInterval(updateClock, 1000);

updateClock();

/**
 *	Tag based
 */

// Initialize an array to store clicked tags
var clickedTagsArray = [];
var tagsArray = [];

function InitializeTagContainer() {
    // Get the container element where you want to append the buttons
    var container = document.getElementById('tags-container');

    // Loop through the array and create buttons
    for (var i = 0; i < tagsArray.length; i++) {

        var id = i;
        var value = tagsArray[i];

        AddTagToContainer(id, value);
    }
}

function AddNewTag(value) {

    var button = AddTagToContainer(tagsArray.length, value);
    // $(button).click();
    // clickedTagsArray.push(value);
    tagsArray.push(value);
}

function AddTagToContainer(id, value) {
    // Create a button element
    var button = document.createElement('button');

    // Set attributes and styles for the button
    button.type = 'button';
    button.id = 'tag-button-' + id;
    button.className = 'btn btn-outline-info text-light tag-button-element';
    button.style.fontSize = '12px';
    button.style.boxShadow = 'none';
    button.dataset.id = id; // Assuming you want to set some data attribute for ID
    button.dataset.tag = value; // Set data-tag attribute with the value from the array

    // Set the button text to the value from the array
    button.textContent = value;
    
    var container = document.getElementById('tags-container');
    // Append the button to the container
    container.appendChild(button);

    $(button).on('click', BindTagClick);
    return $(button);
}

function BindTagClick() {
    // Get the clicked tag's text
    var clickedTag = $(this).data('tag');
    var clickedId = $(this).data('id');
    
    var b_ToToggleButton = true;

    // Add the clicked tag to the array
    if (clickedTag !== undefined && clickedTag !== null) {
        var index = clickedTagsArray.indexOf(clickedTag);

        // Check if the tag is not already in the array
        if (index === -1) {
            if (clickedTagsArray.length < 5)
                clickedTagsArray.push(clickedTag);
            else {
                b_ToToggleButton = false;
                alert("Up to 5 tags")
            }
        } else {
            // Remove the tag from the array
            clickedTagsArray.splice(index, 1);
        }

        // Update the input field by joining the array elements
        var tagsInput = document.querySelector('[data-tags-input="tags-input"]');
        tagsInput.value = clickedTagsArray.join(' ');
        
        // Manage custom tag input field
        var inputElement = document.querySelector('[data-custom-tag-input="custom-tag_input"]');
        if (clickedTagsArray.length == 5) {
            inputElement.setAttribute('readonly', true);
            inputElement.setAttribute('disabled', true);
            inputElement.value = 'Up to 5 tags';
        }
        else {
            inputElement.removeAttribute('readonly');
            inputElement.removeAttribute('disabled');
            inputElement.value = '';
        }
    }


    if (b_ToToggleButton) {
        if ($(this).hasClass('btn-outline-info')) {
            
            $(this).removeClass('btn-outline-info');
            $(this).removeClass('text-light');


            $(this).addClass('btn-info');
            $(this).addClass('text-dark');
        }
        else {
            $(this).addClass('btn-outline-info');
            $(this).addClass('text-light');


            $(this).removeClass('btn-info');
            $(this).removeClass('text-dark');
        }
    }
}
function OnCustomTagInputChanged (event) {

    var charCode = event.charCode;

    // Check if the entered character is a valid one (a-z, A-Z, 0-9, dash or dot)
    if ((charCode >= 48 && charCode <= 57) ||   // 0-9
        (charCode >= 65 && charCode <= 90) ||   // A-Z
        (charCode >= 97 && charCode <= 122) ||  // a-z
        charCode === 13 ||						// Enter
        charCode === 45 ||						// dash
        charCode === 46) {                      // dot (.)

        //

        if (event.which === 13) { // 13 is the key code for Enter
            event.preventDefault(); // Prevent the default behavior of the Enter key

            if ($(this).val().length > 0) {

                // Call your function with the input value
                var inputValue = $(this).val().toLowerCase();

                if (tagsArray.includes(inputValue)) {
                    alert('Tag is already created');
                }
                else {
                    if (clickedTagsArray.length < 5) {
                        AddNewTag(inputValue);

                        // Optionally, clear the input field after pressing Enter
                        $(this).val('');
                    }
                }
            }
        }
    } else {
        event.preventDefault(); // Prevent entering the invalid character
        alert('Only english letters, numbers "-" and "." allowed.');
    }
}
// 

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