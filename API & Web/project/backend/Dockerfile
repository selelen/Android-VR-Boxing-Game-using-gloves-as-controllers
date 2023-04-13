FROM python:3.9.12
ENV PYTHONUNBUFFERED=1
WORKDIR ./backend
COPY requirements.txt /backend/
RUN pip install -r requirements.txt
COPY . /backend/
CMD python manage.py makemigrations && python manage.py migrate && python manage.py runserver 0.0.0.0:8000