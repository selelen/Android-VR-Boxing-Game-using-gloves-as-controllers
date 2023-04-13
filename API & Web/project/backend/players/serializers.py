from rest_framework import serializers

from .models import*

# Serializers


class UserSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = User
        fields = ['id', 'username', 'email']


class PlayerSerializer(serializers.ModelSerializer):
    class Meta:
        model = PlayerInfo
        fields = ['id', 'user_id', 'score']
