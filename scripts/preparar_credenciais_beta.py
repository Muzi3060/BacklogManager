#!/usr/bin/env python3
"""Gera somente o recurso local da beta. Nunca imprime os valores recebidos."""
import argparse, json, os, pathlib, sys

def preparar(destino, ambiente):
    cliente = ambiente.get('MR_BETA_IGDB_CLIENT_ID', '').strip()
    token = ambiente.get('MR_BETA_IGDB_ACCESS_TOKEN', '').strip()
    if not cliente or not token or any(c.isspace() or ord(c) < 32 for c in cliente + token):
        raise ValueError('Publicação beta exige Client ID e Access Token válidos no ambiente privado de preparação.')
    destino = pathlib.Path(destino)
    destino.parent.mkdir(parents=True, exist_ok=True, mode=0o700)
    if destino.is_symlink():
        raise ValueError('Recurso de credenciais não pode ser um link.')
    fd = os.open(destino, os.O_WRONLY | os.O_CREAT | os.O_TRUNC, 0o600)
    with os.fdopen(fd, 'w', encoding='utf-8') as arquivo:
        json.dump({'ClientId': cliente, 'AccessToken': token, 'DaBeta': True}, arquivo)
    os.chmod(destino, 0o600)

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--destino', required=True)
    args = parser.parse_args()
    try:
        preparar(args.destino, os.environ)
    except (ValueError, OSError):
        print('Falha na preparação privada da IGDB; confira o par de credenciais e o destino local.', file=sys.stderr)
        sys.exit(1)
