

function RefreshTaskForDraging() {

    const taskItems = document.querySelectorAll(".task-item");

    taskItems.forEach(u => { u.ondrag = null });

    taskItems.forEach(u => { u.addEventListener("drag", Drag) });

}

function Drag(e) {
    const dragTaskHiddenInput = document.querySelector("#dragTaskId");

    dragTaskHiddenInput.value = e.target.id;
}




function DropTask(memberId,dotnetHelper) {

    const dropDiv = document.getElementById(`${memberId}`);
    const dropZone = document.getElementById(`dropZone${memberId}`);

    dropDiv.addEventListener("drop", Drop);

    async function Drop(e) {
        e.stopPropagation();
        e.preventDefault();        
        const taskId = document.querySelector("#dragTaskId").value;     
        dropZone.style = "display:none";       
        await dotnetHelper.invokeMethodAsync("AssignTaskToMember", taskId.toString(), memberId.toString());
        document.querySelector("#dragTaskId").value = "";

    }


    dropDiv.addEventListener("dragover", allowDrop);
   

    function allowDrop(e) {
        e.stopPropagation();
        dropZone.style = null;
        e.preventDefault();
    }


    dropDiv.addEventListener("dragleave", dragLeave);

    function dragLeave(e) {
        e.stopPropagation();
        dropZone.style = "display:none";
        e.preventDefault();
    }



}
