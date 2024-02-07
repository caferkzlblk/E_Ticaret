//sadece rakam girilmesi gereken alanı kontrol eden fonksiyon

function numberonly(myfield, e, dec) {
    var key; //asci kodu
    var keychar; //asci kodu karaktere çevirip içini dolduracağız

    if (window.event) {
        key = window.event.keyCode;
    }
    else if (e) {
        key = e.which;
    }
    else {
        return true;
    }

    keychar = String.fromCharCode(key); //asci kodunu karaktere cevir

    //control keys bazı tuşlara izin verme
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
        return true;


    }

    //numbers ("012324356789")

    else if ((("012324356789").indexOf(keychar) > -1)) {
        return true;
    }



    else if (dec && (keychar == ".")) {
        myfield.form.elements[dec].focus();
        alert("Sadece Rakam Giriniz");
        return false;
    }

    else {
        alert("Sadece Rakam Giriniz");
        return false;
    }
}