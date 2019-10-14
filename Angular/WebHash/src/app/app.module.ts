import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ArticlesComponent } from './articles/articles.component';
import { ArticleService } from './services/article.service';
import { AuthService } from './auth/auth.service';
import { AuthGuardService } from './auth/auth-guard.service';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FeedConfigurationComponent } from './feed-configuration/feed-configuration.component';
import { ManageNewsComponent } from './manage-news/manage-news.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { SingleArticleComponent } from './single-article/single-article.component';

// Angular Materials
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatInputModule} from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { MainPageComponent } from './main-page/main-page.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthHttpInterceptor } from './auth/auth-http-interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ArticlesComponent,
    FeedConfigurationComponent,
    ManageNewsComponent,
    SignInComponent,
    SingleArticleComponent,
    MainPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    // Angular Materials
    BrowserAnimationsModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    FormsModule,

    // Jwt validation
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:4200'],
      }
    }),
  ],
  providers: [
    ArticleService,
    AuthService,
    AuthGuardService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }


export function tokenGetter() {
  return localStorage.getItem('auth_token');
}
