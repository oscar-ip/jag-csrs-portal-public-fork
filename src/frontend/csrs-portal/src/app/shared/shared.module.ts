import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ConfigModule } from 'app/config/config.module';
import { NgxMaskModule } from 'ngx-mask';
import { AlertComponent } from './components/alert/alert.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { PageHeaderComponent } from './components/page-header/page-header.component';
import { PageComponent } from './components/page/page.component';
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component';
import { NgxBusyModule } from './modules/ngx-busy/ngx-busy.module';
import { NgxMaterialModule } from './modules/ngx-material/ngx-material.module';
import { NgxProgressModule } from './modules/ngx-progress/ngx-progress.module';
import { CapitalizePipe } from './pipes/capitalize.pipe';
import { DefaultPipe } from './pipes/default.pipe';
import { FormatDatePipe } from './pipes/format-date.pipe';
import { PhonePipe } from './pipes/phone.pipe';
import { ReplacePipe } from './pipes/replace.pipe';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { YesNoPipe } from './pipes/yes-no.pipe';

@NgModule({
    declarations: [
        CapitalizePipe,
        DefaultPipe,
        FormatDatePipe,
        PhonePipe,
        ReplacePipe,
        SafeHtmlPipe,
        YesNoPipe,
        PageComponent,
        PageHeaderComponent,
        AlertComponent,
        HeaderComponent,
        FooterComponent,
        ConfirmDialogComponent,
        FooterComponent
    ],
    imports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        NgxBusyModule,
        NgxMaterialModule,
        NgxMaskModule.forRoot(),
        NgxProgressModule,
        ConfigModule,
        TranslateModule,
        MatDialogModule,
    ],
    exports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        NgxBusyModule,
        NgxMaterialModule,
        NgxMaskModule,
        NgxProgressModule,
        CapitalizePipe,
        DefaultPipe,
        FormatDatePipe,
        PhonePipe,
        ReplacePipe,
        SafeHtmlPipe,
        YesNoPipe,
        PageComponent,
        PageHeaderComponent,
        AlertComponent,
        ConfigModule,
        HeaderComponent,
        FooterComponent
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule { }
