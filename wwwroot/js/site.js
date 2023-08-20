let selectedTags = [];
let tagColors = {};
let allTags = [];

window.onload = async function () {
    allTags = await getTagList();
    $('#tags').select2({
        placeholder: "Add tags...",
        tags: true,
        data: allTags
    });
    addTagButtons(allTags);
    await showTaggedMessages(allTags);
};

async function getTagList() {
    let response = await fetch('/Message/GetAllTags/', {
        method: 'GET',
    });
    return response.json();
}

function addTagButtons(tags) {
    let myTagsList = document.getElementById('my-tags');
    tags.forEach(function (tag, index) {
        let btn = getNewTagButton(tag, index);
        document.getElementById('my-tags').appendChild(btn);
    });
}

function getNewTagButton(tag, index) {
    let btnColor = getButtonColor(index);
    tagColors[tag] = btnColor;
    let btn = document.createElement("button");
    setButtonOptions(btn, btnColor);
    btn.appendChild(document.createTextNode(tag));
    btn.addEventListener('click', async function () {
        changeTagButtonStatus(btn, btnColor)
        await selectTag(tag);
    });
    return btn;
}

function getButtonColor(index) {
    let btnColors = ['primary', 'secondary', 'success', 'danger', 'warning', 'info'];
    return btnColors[index % btnColors.length];
}

function setButtonOptions(btn, btnColor) {
    btn.className = `btn btn-outline-${btnColor} rounded-pill px-3`;
    btn.style = "flex-grow: 1;";
    btn.type = "button";
}

function changeTagButtonStatus(btn, btnColor) {
    if (btn.classList.contains(`btn-outline-${btnColor}`)) {
        btn.classList.remove(`btn-outline-${btnColor}`);
        btn.classList.add(`btn-${btnColor}`);
    } else {
        btn.classList.remove(`btn-${btnColor}`);
        btn.classList.add(`btn-outline-${btnColor}`);
    }
}

async function selectTag(tag) {
    const index = selectedTags.indexOf(tag);
    index > -1 ? selectedTags.splice(index, 1) : selectedTags.push(tag);
    selectedTags.length === 0 ? await showTaggedMessages(allTags) : await showTaggedMessages(selectedTags);
}

async function showTaggedMessages(tags) {
    document.getElementById('chat-box').innerHTML = '';
    let response = await getTaggedMessages(tags);
    let messages = await response.json();
    messages.forEach(message => {
        addMessageToChat(message.text, message.tags);
    });
}

async function getTaggedMessages(tags) {
    let response = await fetch('/Message/GetTaggedMessages/', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ SelectedTags: tags })
    });
    return response;
}

function addMessageToChat(text, tags) {
    var chatBox = document.getElementById('chat-box');
    var cardDiv = createDiv('card mb-3');
    var cardBodyDiv = createDiv('card-body');
    var textSpan = createSpanWithClassAndText('text', text);
    var tagsDiv = createDiv('tags d-flex gap-2');
    AddTagsToMessage(tagsDiv, tags);
    cardBodyDiv.appendChild(textSpan);
    cardBodyDiv.appendChild(tagsDiv);
    cardDiv.appendChild(cardBodyDiv);
    chatBox.appendChild(cardDiv);
}

function createDiv(className) {
    var div = document.createElement('div');
    div.className = className;
    return div;
}

function createSpanWithClassAndText(className, text) {
    var span = document.createElement('span');
    span.className = className;
    span.innerText = text;
    return span;
}

function AddTagsToMessage(tagsDiv, tagArray) {
    console.log(tagArray);
    tagArray.forEach(function (tag) {
        var tagSpan = document.createElement('span');
        var cleanTag = tag.trim();
        var tagColor = tagColors[cleanTag] || 'primary';
        tagSpan.className = `badge bg-${tagColor} rounded-pill`;
        tagSpan.innerText = cleanTag;
        tagsDiv.appendChild(tagSpan);
    });
}

document.getElementById('send-message-button').addEventListener('click', async (event) => {
    event.preventDefault();
    let messageText = document.getElementById('message-text').value;
    let tags = $('#tags').val();
    if (messageText !== '') {
        await AddNewMessage(messageText, tags);
        location.reload();
    }
});

async function AddNewMessage(messageText, tags) {
    await fetch('/Message/AddNewMessage/', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Text: messageText, Tags: tags })
    });
}