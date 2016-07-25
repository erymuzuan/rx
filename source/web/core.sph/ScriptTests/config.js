define(["knockout"], function (ko) {
    var imageSettings = {
        imageBasePath: "~/content/images/photos/",
        unknownPersonImageSource: "unknown_person.jpg"
    },
        routes = [
{
    "role": null,
    "groupName": "default",
    "route": "",
    "moduleId": "viewmodels/dev.home",
    "title": null,
    "nav": false,
    "icon": null,
    "caption": null,
    "settings": null,
    "showWhenLoggedIn": true,
    "isAdminPage": false,
    "startPageRoute": null
},
{
    "role": "everybody",
    "groupName": null,
    "route": "custom-paging",
    "moduleId": "viewmodels/custom-paging",
    "title": "Custom Paging for Ima",
    "nav": true,
    "icon": "fa fa-pagelines",
    "caption": "Custom Paging for Ima",
    "settings": null,
    "showWhenLoggedIn": false,
    "isAdminPage": false,
    "startPageRoute": null
}],
        startModule = "dev.home";


    return {
        debugEnabled: ko.observable(true),
        lang: "en-US",
        imageSettings: imageSettings,
        userName: "admin",
        isAuthenticated: true,
        routes: routes,
        startModule: startModule,
        stateOptions: [],
        departmentOptions: [{ "Name": "Finance" }, { "Name": "Human Resource" }, { "Name": "Information Technology" }],
        applicationFullName: "Engineering Team Development",
        applicationName: "DevV1",
        roles: ["administrators", "developers"],
        allRoles: ["administrators", "clerk", "developers", "PendaftaranAdmin"],
        profile: { "UserName": "admin", "FullName": "admin", "Designation": "Developers", "Telephone": "-", "Mobile": null, "RoleTypes": "administrators,developers", "StartModule": "dev.home", "Email": "admin@yourcompany.com", "Department": null, "HasChangedDefaultPassword": true, "Language": "en-US", "IsLockedOut": false, "CreatedBy": "admin", "Id": "admin", "CreatedDate": "2016-01-08T09:31:39.6547776+08:00", "ChangedBy": "admin", "ChangedDate": "2016-02-23T08:11:52.4715357+08:00", "WebId": "12dcc69c-d00b-4160-939d-23b974f6dcc7" }
    };
});
