$(document).ready(function () {
    // alert("document).ready");
    //   say("DocumentReady");
    //сразу установим ссылку
    var vvs = $("#TblKfId option:selected").val();
      //   alert(vvs);
    //   UstnPrhdSlctZnc(vvs);
    $('#TblKfId').on('change', fncchangetblname);
    //$('#TblKfId').on('select', fncchangetblname);
    UstnPrhdSlctZnc(vvs);          
});

function UstnPrhdSlctZnc(vslct) {
 
  //   alert(vslct);
     izmhrf("iskfot",vslct);
     izmhrf("isipm",vslct);
 }


//
function izmhrf(hid,znc)
{
    var elm = document.getElementById(hid);
    var hatrb = elm.getAttribute("href");
   // alert(hatrb);
    var pos = hatrb.lastIndexOf("/");
    var res = hatrb.slice(0, pos + 1);
    //alert(res);
    var satr = res.concat(znc);
    //alert(satr);
    elm.setAttribute("href", satr);
}


function fncchangetblname() {
    var val = this.value;
    UstnPrhdSlctZnc(val)
}

/*
function UstnPrhdSlctZnc2(vslct) {
    //  alert('UstnPrhdSlctZnc');
    var tga = document.getElementsByTagName("a");
    var dln = tga.length;
    for (var i = 0; i < dln; i++) {
        var ancr = tga[i];
        var hatrb = ancr.getAttribute("href");
        if (hatrb != null) {
            var iko = hatrb.indexOf("IsprStrctKfOtrz")
            if (iko > 0) {
                satr = "/TabKofIsp/IsprStrctKfOtrz/" + vslct;
                ancr.setAttribute("href", satr);
            }
            var iip = hatrb.indexOf("IsprStrctIndPom")
            if (iip > 0) {
                //  satr = "/TabKofIsp/IsprStrctIndPom/" + vslct;
                satr = "/IndxPmSzdKr/IsprStrctIndPom/" + vslct;
                ancr.setAttribute("href", satr);
            }
        }
    }
};
*/
///document.getElementById(id_string) → Return a element object. Returns null if not found.
// document.getElementsByTagName(tag_name) → Return a live HTMLCollection. The tag_name are {"div", "span", "p", …}.
//document.getElementsByClassName("class_values") → Return a live HTMLCollection. The class_values can be multiple classes separated by 
//document.getElementsByName("name_value") → Return a live HTMLCollection, of all elements that have the name="name_value" attribute ＆ value 
//document.querySelector(css_selector) → Return a non-live HTMLCollection, of the first element that match the CSS selector css_selector. The css_selector is a string of CSS syntax, and can be several selectors separated by a comma.
// document.querySelectorAll(css_selector) → Return a non-live HTMLCollection, of elements that match the CSS selector css_selector. The css_selector is a string of CSS syntax, and can be several selectors separated by a comma.