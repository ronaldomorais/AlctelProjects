var CustomDateDAO = function() {
    this._customDate = new Date();    
}

CustomDateDAO.prototype.getStringMonth = function() {
    let month = this._customDate.getMonth() + 1;

    if (month < 10) { return `0${month}` }
    return month;
}

CustomDateDAO.prototype.getStringDate = function() {
    let day = this._customDate.getDate();

    if (day < 10) { return `0${day}` }
    return day;
}

CustomDateDAO.prototype.getStringHours = function() {
    let hours = this._customDate.getHours();

    if (hours < 10) { return `0${hours}` }
    return hours;
}

CustomDateDAO.prototype.getStringMinutes = function() {
    let minutes = this._customDate.getMinutes();

    if (minutes < 10) { return `0${minutes}` }
    return minutes;
}

CustomDateDAO.prototype.getStringSeconds = function() {
    let seconds = this._customDate.getSeconds();

    if (seconds < 10) { return `0${seconds}` }
    return seconds;
}

CustomDateDAO.prototype.getStringFullYear = function() {
    let years = this._customDate.getFullYear() ;
    return years;
}

CustomDateDAO.prototype.getNowTimeHHMMSS = function() {
    return `${this.getStringHours()}:${this.getStringMinutes()}:${this.getStringSeconds()}`;
}

CustomDateDAO.prototype.getYYYYMMDD = function() {
    return `${this.getStringFullYear()}-${this.getStringMonth()}-${this.getStringDate()}`;     
}

module.exports = function() {
    return CustomDateDAO;
}

// module.exports = CustomDateDAO;