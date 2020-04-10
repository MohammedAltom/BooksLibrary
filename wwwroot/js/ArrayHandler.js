function array_handle(element, hidden_id) {
    var id = element.id;
    var hidden = document.getElementById(hidden_id);
    var arr = document.getElementById(hidden_id).value.split(',');
    if (check_val(id, arr))
    {
        hidden.value = hidden.value.replace(id+",", '');
    } else {
        hidden.value = hidden.value + id+",";
    }
}

function check_val(val, arr) {
    for (var i = 0; i < arr.length ; i++) {
        if (arr[i] == val) {
            return true;
        }
    }
    return false;
}