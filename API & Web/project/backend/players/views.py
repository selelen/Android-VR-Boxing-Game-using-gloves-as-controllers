from pickle import TRUE
from django.http import HttpResponse
from django.contrib.auth.models import User

from rest_framework import viewsets
from django_filters.rest_framework import DjangoFilterBackend
from rest_framework import filters

from .models import *
from .serializers import *
from django.contrib.auth import authenticate, login, logout

# view to manage the players score


def manage_score(request, username, is_win, difficult_level):
    # first we obtain the player from the given player id
    user = User.objects.filter(username=username).first()
    player = PlayerInfo.objects.filter(user_id=user).first()
    # if the player won the match, depending on the difficult level, we add a certain amount of score
    if is_win == "win":
        if difficult_level == 1:
            player.addScore(10)
        elif difficult_level == 2:
            player.addScore(20)
        elif difficult_level == 3:
            player.addScore(30)

    # if the player lost, depending on the difficult level, we substract a certain amount of score
    # if the player's score is equals to 0, or if after the substraction the score is less to 0, we leave it at 0
    else:
        if player.score != 0:
            if difficult_level == 1:
                if player.score-5 >= 0:
                    player.substractScore(5)
                else:
                    player.substractScore(player.score)
            elif difficult_level == 2:
                if player.score-8 >= 0:
                    player.substractScore(8)
                else:
                    player.substractScore(player.score)
            elif difficult_level == 3:
                if player.score-10 >= 0:
                    player.substractScore(10)
                else:
                    player.substractScore(player.score)

    return HttpResponse()

# function to create a new player with the given name, email and password


def create_player(request, name, email, password):
    # Check if the username already exists at the database
    if User.objects.filter(username=name).first():
        return HttpResponse("Username already taken")

    # Check if the given email already exists in the database

    elif User.objects.filter(email=email).first():
        return HttpResponse("Email already exists")
    else:
        try:
            user = User.objects.create_user(
                username=name, email=email, password=password)
            user.save()
            print('created user')
            player = PlayerInfo.objects.create(user_id=user)
            print('created player')
            player.save()
            return HttpResponse("Successful")
        except:
            return HttpResponse("User already exists")

# function to login


def login_user(request, username, password):
    user = authenticate(request, username=username, password=password)
    print(user)
    if user is not None:
        login(request, user)
        return HttpResponse("Successful")
    else:
        return HttpResponse("Invalid login")


# function to log out
def logout_user(request):
    logout(request)
    return HttpResponse()

# Serializers


class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer
    filter_backends = [DjangoFilterBackend]
    filterset_fields = ['id', 'username'] 

class PlayerViewSet(viewsets.ModelViewSet):
    queryset = PlayerInfo.objects.all()
    serializer_class = PlayerSerializer
    filter_backends = [DjangoFilterBackend]
    filterset_fields = ['id', 'user_id']


#obtain ranking

class RankingViewSet(viewsets.ModelViewSet):
    queryset = PlayerInfo.objects.all()
    serializer_class = PlayerSerializer
    filter_backends = [filters.OrderingFilter]
    ordering=["-score"] #order by descending score
