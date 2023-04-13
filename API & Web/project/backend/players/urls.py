from django.urls import path, include
from rest_framework import routers


from . import views

app_name = 'players'

router = routers.DefaultRouter()


router.register(r'players', views.PlayerViewSet)
# ex: players/api/players #filterEx: players/api/players?id=1 /or/ #filterEx: players/api/players?user_id=1

router.register(r'users', views.UserViewSet)
# ex: players/api/users   #filterEx: players/api/users?id=1  /or/ #filterEx: players/api/user?username=selena

router.register(r'ranking', views.RankingViewSet)  
# ex: players/api/ranking

urlpatterns = [
    path('api/', include(router.urls)),
    #ex: players/login/name/password
    path('login/<str:username>/<str:password>', views.login_user, name="login"),
    #ex: players/logout
    path('logout/', views.logout_user, name="logout"),
    # ex: /players/score/win/5/1
    path('score/<str:username>/<str:is_win>/<int:difficult_level>/',
         views.manage_score, name='score'),

    # ex: players/create/selena/selena@email.es/admin
    path('create/<str:name>/<str:email>/<str:password>',
         views.create_player, name='create_player'),
]
