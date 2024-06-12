export const mapNumberToMonthValue = {
    1: "January",
    2: "February",
    3: "March",
    4: "April",
    5: "May",
    6: "June",
    7: "July",
    8: "August",
    9: "September",
    10: "October",
    11: "November",
    12: "December",
} 

export const AccessTypeArray = [
    { value: 1, typeName: 'Edit'},
    { value: 2, typeName: 'Read Only'}
]

export const getFormattedDate = (date) => {
    let monthValue = date.getMonth();
    return `${mapNumberToMonthValue[++monthValue]} ${date.getDate()} ${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
  }

export function getAccessTypeName (accessNumber) {
    const access = AccessTypeArray.filter(x => x.value === accessNumber)[0];
    
    return access.typeName;
}

export function getSum(items, prop){
    return items.reduce( function(a, b){
        return a + b[prop];
    }, 0);
};