export function MN_CheckThatElementIsInListAndToogle(idElementsArray, targetId, className, ignoredIdElementsArray = []) {
    document.addEventListener('click', (e) => {
        let elementId = e.target.id;

        if(ignoredIdElementsArray.length > 0 && ignoredIdElementsArray[0].includes(elementId))
            return;
            
        if (idElementsArray[0].includes(elementId)) {
            MN_ToogleClassElement(targetId, className);
            return;
        }

        var element = document.getElementById(targetId);
        MN_RemoveClassFromElement(element, className);
    });
}

export function MN_ToogleClassElement(targetId, className) {

    var element = document.getElementById(targetId);
    if (element.classList.contains(className)) {
        MN_RemoveClassFromElement(element, className);
        return;
    }

    MN_AddClassToElement(element, className);
}

export function MN_AddClassToElement(element, className) {
    element.classList.add(className);
}

export function MN_RemoveClassFromElement(element, className) {

    element.classList.remove(className);
}