const createTodoForm = document.getElementById('create-todo-form');
const createTodoTitle = document.getElementById('create-todo-title');
const createTodoDescription = document.getElementById('create-todo-description');
const createTodoFinishUntil = document.getElementById('create-todo-finish-until');

function init() {
    const checkboxes = document.querySelectorAll('.list__item__checkbox');
    checkboxes.forEach(addCheckboxListener);

    createTodoForm.addEventListener('submit', handleCreateTodoFormSubmit);
}

function handleCreateTodoFormSubmit(event) {
    event.preventDefault();

    const newTodo = {
        Title: createTodoTitle.value,
        Description: createTodoDescription.value,
        FinishUntil: createTodoFinishUntil.value.replace(" ", "T")
    };

    createNewTodo(newTodo);
}

async function createNewTodo(newTodo) {
    const response = await fetch(createUrl, {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(newTodo),
    })
        .then((response) => response.text())
        .then((html) => {
            const template = document.createElement("template");
            template.innerHTML = html.trim();

            const listItem = template.content.querySelector(".list__item");

            const list = document.querySelector(".list");
            list.prepend(listItem);

            const newCheckbox = listItem.querySelector(".list__item__checkbox");
            addCheckboxListener(newCheckbox);
            createTodoForm.reset();
        })
        .catch(error => console.error(error));
}

function addCheckboxListener(checkbox) {
    checkbox.addEventListener("change", handleCheckboxChange);
}

async function handleCheckboxChange(event) {
    const listItem = event.target.closest(".list__item");
    const todoId = listItem.dataset.id;
    const isCompleted = event.target.checked;
    const loadingIcon = listItem.querySelector(".loading");
    loadingIcon.style.display = "block";

    const updateTodo = {
        Id: todoId,
        IsCompleted: isCompleted,
    };

    try {
        await fetch(updateUrl, {
            method: "PUT",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
            },
            body: JSON.stringify(updateTodo),
        });

        updateListItem(listItem, isCompleted);
        reorderListItems();
    } catch (error) {
        console.error(error);
    } finally {
        loadingIcon.style.display = "none";
    }
}

function updateListItem(listItem, isCompleted) {
    listItem.classList.toggle("list__item--completed", isCompleted);
    listItem.setAttribute('data-is-completed', isCompleted);
    listItem.querySelector(".list__item__completed").textContent = isCompleted
        ? "Completed"
        : "Not Completed";

    const completedElement = listItem.querySelector(".list__item__completed");
    completedElement.textContent = isCompleted ? "Completed" : "Not Completed";
    completedElement.classList.toggle("completed", isCompleted);
    completedElement.classList.toggle("not-completed", !isCompleted);
}

function reorderListItems() {
    const list = document.querySelector(".list");
    const listItems = Array.from(list.children);

    const completedItems = listItems.filter((item) => item.getAttribute("data-is-completed") === "true");
    const uncompletedItems = listItems.filter((item) => item.getAttribute("data-is-completed") !== "true");

    list.innerHTML = "";
    uncompletedItems.forEach((item) => list.appendChild(item));
    completedItems.forEach((item) => list.appendChild(item));
}

async function handleDeleteButtonClick(event) {
    const listItem = event.target.closest(".list__item");
    const todoId = listItem.dataset.id;

    try {
        const response = await fetch(`${deleteUrl}/?id=${todoId}`, {
            method: "DELETE",
        });

        if (response.ok) {
            listItem.remove();
        }
    } catch (error) {
        console.error(error);
    }
}

document.addEventListener("DOMContentLoaded", init);

