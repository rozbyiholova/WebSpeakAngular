import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { DTO } from '../../../models/DTO';

@Component({
    selector: 'account-index',
    templateUrl: './index.component.html'
})
export class AccountIndexComponent implements OnInit {
    settings: DTO;
    submitted = false;
    enableNativeLang: boolean;
    enableSound: boolean;
    enablePronounceNativeLang: boolean;
    enablePronounceLearnLang: boolean;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.loadSettings();   
    }

    loadSettings() {
        this.dataService.getSettings()
            .subscribe((data: DTO) => {
                this.settings = data;
                if (this.settings.enableNativeLang.includes("true")) {
                    this.enableNativeLang = true;
                }
                if (this.settings.enableSound.includes("true")) {
                    this.enableSound = true;
                }
                if (this.settings.enablePronounceNativeLang.includes("true")) {
                    this.enablePronounceNativeLang = true;
                }
                if (this.settings.enablePronounceLearnLang.includes("true")) {
                    this.enablePronounceLearnLang = true;
                }
            });
    }

    setSettings() {
        this.submitted = true;
        this.settings.enableNativeLang = this.enableNativeLang.toString();
        this.settings.enableSound = this.enableSound.toString();
        this.settings.enablePronounceNativeLang = this.enablePronounceNativeLang.toString();
        this.settings.enablePronounceLearnLang = this.enablePronounceLearnLang.toString();

        this.dataService.setSettings(this.settings).subscribe();
    }
}