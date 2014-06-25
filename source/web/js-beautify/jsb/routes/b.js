var express = require('express');
var router = express.Router();

/* GET home page. */
router.get('/', function(req, res) {
  res.render('b', { title: 'Using js beatifier' });
});

module.exports = router;
