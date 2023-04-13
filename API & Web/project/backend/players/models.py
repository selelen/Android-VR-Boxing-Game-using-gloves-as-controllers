from django.db import models
from django.contrib.auth.models import User


class PlayerInfo(models.Model):
    user_id = models.ForeignKey(User, on_delete=models.CASCADE)
    score = models.IntegerField(default=0)

    def __str__(self):
        return self.user_id.username

    def addScore(self, scoreToAdd):
        self.score += scoreToAdd
        self.save()

    def substractScore(self, scoreToSubstract):
        self.score = self.score-scoreToSubstract
        self.save()
