import { Constants } from './Constants';
import { TestInfo } from './TestInfo';

export class FillMethod {
    public static LoadPictures(info: TestInfo): void {
        const setting: Object = info.setting;
        const picturesCount: number = +setting['images'];
        if (picturesCount < 1) { return; }

        let testImages = document.getElementsByClassName(Constants.TEST_IMAGES)[0] as HTMLElement;
        testImages.style.display = "block";
        this.empty(Constants.TEST_IMAGES);

        let selectedCategories = new Array();

        const categories: any[] = info.categories;
        const categoriesLength = categories.length;
        selectedCategories[0] = categories[info.currentIndex];
        let i = 1;
        while (i < picturesCount) {
            const random = this.getRandomInt(0, categoriesLength);
            const category = categories[random];
            if (selectedCategories.indexOf(category) === -1) {
                selectedCategories.push(category);
                i++;
            }
        }

        this.shuffle(selectedCategories);
        for (let j = 0; j < selectedCategories.length; j++) {

            let category = selectedCategories[j];

            let radio: HTMLInputElement = document.createElement("input");
            radio.type = "radio";
            const idText = "rd" + j;
            radio.id = idText;
            let label: HTMLLabelElement = document.createElement("label");
            label.htmlFor = idText;

            let img: HTMLImageElement = document.createElement("img");
            img.src = category.picture.toString();
            img.className = "img-fluid";
            img.dataset["answer"] = category.translation;

            label.appendChild(img);
            testImages.appendChild(radio);
            testImages.appendChild(label);
        }

        this.setSelectionOfOneElement(Constants.TEST_IMAGES, Constants.SELECTED_PICTURE);
    }

    public static LoadText(info: TestInfo) {
        const textsCount: number = +info.setting['words'];
        if (textsCount < 1) { return; }

        let test_word = document.getElementsByClassName(Constants.TEST_WORD)[0] as HTMLElement;
        this.empty(Constants.TEST_WORD);

        let indexes = new Array();
        indexes[0] = info.currentIndex;

        let i = 1;
        while (i < textsCount) {
            let randomTextIndex = this.getRandomInt(0, textsCount + 1);
            if (indexes.indexOf(randomTextIndex) === -1) {
                indexes.push(randomTextIndex);
                i++;
            }
        }

        this.shuffle(indexes);
        for (let j = 0; j < indexes.length; j++) {

            var radio = document.createElement('input');
            radio.type = "radio";
            let IdText = "rd" + j;
            radio.id = IdText;
            var label = document.createElement('label');
            label.htmlFor = IdText;

            let text = info.categories[indexes[j]].translation;
            let h3 = document.createElement("h3");
            let textNode = document.createTextNode(text);
            h3.appendChild(textNode);
            label.appendChild(h3);
            test_word.appendChild(radio);
            test_word.appendChild(label);

            this.setSelectionOfOneElement(Constants.TEST_WORD, Constants.SELECTED_TEXT);
        }
    }

    private static empty(className: string): void {
        let elements = document.getElementsByClassName(className);
        for (let i = 0; i < elements.length; i++) {
            elements[i].innerHTML = '';
        }
    }

    private static getRandomInt(min: number, max: number) {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    private static shuffle(a: any[]) {
        for (let i = a.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [a[i], a[j]] = [a[j], a[i]];
        }
        return a;
    }

    private static siblingsBySelector(element: HTMLElement, selector?: string): HTMLElement[] {

        if (!selector) {
            selector = element.tagName;
        }
        let children = element.parentNode.querySelectorAll(selector);

        return Array.prototype.filter.call(children, (child: any) => child != element && !this.isDescendant(element, child));
    }

    private static isDescendant(parent: HTMLElement, child: HTMLElement): boolean {
        let node = child.parentNode;
        while (node != null) {
            if (node == parent) {
                return true;
            }
            node = node.parentNode;
        }
        return false;
    }

    private static setSelectionOfOneElement(selector: string, operatingClass: string) {
        //hide radio buttons
        const radioElements = document.querySelectorAll(`.${selector} [input="radio"]`);
        radioElements.forEach(element => element.classList.add("input_hidden"));

        //add selection class on click and remove it from siblings
        const labelsList = document.querySelectorAll(`.${selector} label`);
        labelsList.forEach(element => element.addEventListener("click", (event) => {
            //element --> label; target --> image || text
            let target = event.target as HTMLElement;
            target.classList.add(operatingClass);
            this.siblingsBySelector(element as HTMLElement, target.tagName).forEach((sibling: HTMLElement) => {
                sibling.classList.remove(operatingClass);
            });
        }));
    }
}