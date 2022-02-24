
function ConfirmDelete(uniqueId, isDeleted)
{
    var ConfirmDelete1 = 'ConfrmDelete_' + uniqueId;
    var Delete = 'Delete_' + uniqueId;
    // will not work because there is style.display = "none" on span of (Confirmation)
    //var ConfirmDelete = document.getElementById("ConfirmDelete_" + uniqueId);
    //var Delete = document.getElementById("Delete_" + uniqueId);
    //if (isDeleted) {
    //    Delete.style.display = "none";
    //    ConfirmDelete.style.display = "block";
    //}
    //else
    //{
    //    Delete.style.display = "block";
    //    ConfirmDelete.style.display = "none";
    //}
    if (isDeleted) {
        $('#' + Delete).hide();
        $('#' + ConfirmDelete1).show();
    }
    else {
        $('#' + Delete).show();
        $('#' + ConfirmDelete1).hide();
    }
}