@model Project.MVC.ViewModels.VehicleModelVM

<h1>Create New Vehicle Model</h1>

<form asp-action="Create">
    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Abbreviation"></label>
        <input asp-for="Abbreviation" class="form-control" />
        <span asp-validation-for="Abbreviation" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="MakeId"></label>
        <select asp-for="MakeId" asp-items="@(new SelectList(ViewBag.Makes, "Id", "Name"))" class="form-control"></select>
        <span asp-validation-for="MakeId" class="text-danger"></span>
    </div>
    <button type="submit" class="btn btn-primary">Create</button>
</form>