function getDiscussionBoard()
{
    loadPage();
}

getDiscussionBoard();

function loadPage()
{
    fetch("DiscussionBoardControl.html").then(rs => rs.text()).then(d => {
        loadDiscussionPosts(d);
    });
}

function loadDiscussionPosts(control)
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
        DiscussionBoardId: GetFromQueryString("discussionBoardId"),
        Method: "Get"
    };

    fetchFunction("Discussion", data).then(d => {

        for (var i = 0; i < d.Posts.length; i++) {
            var message = d.Posts[i];
            var id = message.Id;

            document.getElementById("pnlDiscussionBoard").innerHTML += control.replace(/_x/g, id).replace(/x_/g, id);
            document.getElementById(id+"publisherName").textContent = message.Name;
            document.getElementById(id +"publishedDate").textContent = message.PublishedDate;
            document.getElementById(id +"postTitle").textContent = message.Title;
            document.getElementById(id +"postDescription").textContent = message.Description;
            document.getElementById(id +"publisherImage").src = (message.Photo == "") ? "Content/images/photo.jpg" : message.Photo;

            if (message.IsAuthor) {
                document.getElementById(id + "PanelEditDeleteDiscussion").style.visibility = "visible";
                document.getElementById(id + "postUpdateTitle").textContent = message.Title;
                document.getElementById(id + "postUpdateDescription").textContent = message.Description;

            var el = document.getElementById(id + "postUpdateTitle");
            }
        }
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });

}

function btnUpdateDiscussion(postId)
{
    //----------------Disable page----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
        DiscussionBoardId: GetFromQueryString("discussionBoardId"),
        DiscussionPostId: postId,
        NewPostTitle: document.getElementById(postId + "postUpdateTitle").textContent,
        NewPostDescription: document.getElementById(postId + "postUpdateDescription").textContent,
        Method: "Update"
    };

    fetchFunction("Discussion", data).then(d => {

        var result = d.Result;
        //TODO

        Navigate("DiscussionBoardPage.html");
        
    });


}

function btnDeleteDiscussion(postId) {
    //----------------Disable page----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
        DiscussionBoardId: GetFromQueryString("discussionBoardId"),
        DiscussionPostId: postId,
        Method: "Delete"
    };

    fetchFunction("Discussion", data).then(d => {

        var result = d.Result;
        //TODO

        Navigate("DiscussionBoardPage.html");
    });

}

function btnAddDiscussionPost()
{
    if (validateForm()) {
        //----------------Disable page----------------------------
        document.getElementById("disabled-div").style.display = "block";
        //---------------------------------------------------------

        var postId = 1;

        const data = {
            CourseInstanceId: GetFromQueryString("courseInstanceId"),
            ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
            DiscussionBoardId: GetFromQueryString("discussionBoardId"),
            NewPostTitle: document.getElementById("newTitle").value,
            NewPostDescription: document.getElementById("newDescription").value,
            Method: "Add"
        };

        fetchFunction("Discussion", data).then(d => {

            document.getElementById("newTitle").value = "";
            document.getElementById("newDescription").value = "";
            Navigate("DiscussionBoardPage.html");

        });

        $('#AddNewDiscussion').modal('hide');
    }
}
function validateForm() {
   
    if (document.getElementById("newTitle").value == "") {
        document.getElementById("AddddModalMessage").textContent ="Title must be filled out"
        return false;
    }
    if (document.getElementById("newDescription").value == "") {
        document.getElementById("AddddModalMessage").textContent = "Description must be filled out"

        return false;
    }
    return true;
}