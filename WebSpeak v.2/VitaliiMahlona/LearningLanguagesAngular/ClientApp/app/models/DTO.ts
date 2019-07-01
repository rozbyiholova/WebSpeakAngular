import { Languages } from "./Languages"

export class DTO {
    constructor(
        public id: number,
        public idLangNative: number,
        public idLangLearn: number,
        public languagesList: Languages[],
        public wordNativeLang: string,
        public wordLearnLang: string,
        public picture: string,
        public enableNativeLang: string,
        public enableSound: string,
        public enablePronounceNativeLang: string,
        public enablePronounceLearnLang: string,
        public sound: string,
        public pronounceLearn: string,
        public pronounceNative: string,
        public categoryId?: number,
        public subCategoryId?: number) { }
}