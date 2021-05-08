import { Component, LOCALE_ID, Inject  } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'app';
  languageList = [
    { code: 'en', label: 'English' },
    { code: 'bg', label: 'Bulgarian' },
  ];
  
  constructor(@Inject(LOCALE_ID) protected localeId: string) { }
}