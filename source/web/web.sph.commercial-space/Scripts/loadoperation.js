function LoadOperation() {
    var self = this;
    self.hasNextPage = false;
    self.itemCollection = [];
    self.page = 1;
    self.size = 40;
    self.rows = null;
    self.nextPageToken = "";
    self.previousPageToken = "";
}