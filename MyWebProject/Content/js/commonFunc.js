function isNullOrEmpty(check) {
    if (check == null || check == '' || typeof (check) == 'undefined') {
        return true;
    }
    return false;
}