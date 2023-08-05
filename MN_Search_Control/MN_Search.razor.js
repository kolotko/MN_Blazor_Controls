import { MN_CheckThatElementIsInListAndToogle, MN_RemoveClassFromElement } from "./lib/MN_Libman-Files/common-script.js";

export function MN_Search_CheckThatElementIsInListAndToogle(idElementsArray, targetId, className) {
    MN_CheckThatElementIsInListAndToogle(idElementsArray, targetId, className);
}

export function MN_Search_InputEnterEvent(inputId, targetId, className) {
    var input = document.getElementById(inputId);

    input.addEventListener("keypress", function (event) {
        if (event.key === "Enter") {
            event.preventDefault();

            var element = document.getElementById(targetId);
            MN_RemoveClassFromElement(element, className);
        }
    });
}